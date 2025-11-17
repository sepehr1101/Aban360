using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class ConsumptionManagementQueryService : AbstractBaseConnection, IConsumptionManagementQueryService
    {
        public ConsumptionManagementQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ConsumptionManagementHeaderOutputDto, ConsumptionManagementDataOutputDto>> Get(ConsumptionManagementInputDto input)
        {
            string reportTitle = string.Concat(ReportLiterals.ConsumptionManagerDetail, "-", (input.IsOlgoo ? ReportLiterals.Olgoo : ReportLiterals.ContractualCapacity));
            string query = GetQuery();
            IEnumerable<ConsumptionManagementDataOutputDto> data = await _sqlReportConnection.QueryAsync<ConsumptionManagementDataOutputDto>(query, input, null, 600);
            ConsumptionManagementHeaderOutputDto header = new ConsumptionManagementHeaderOutputDto()
            {
                FromConsumptionAverage = input.FromConsumptionAverage,
                ToConsumptionAverage = input.ToConsumptionAverage,

                FromBaseDateJalali = input.FromBaseDateJalali,
                ToBaseDateJalali = input.ToBaseDateJalali,

                FromComparisonDateJalali = input.FromComparisonDateJalali,
                ToComparisonDateJalali = input.ToComparisonDateJalali,

                FromMultiplier = input.FromMultiplier,
                ToMultiplier = input.ToMultiplier,

                IsOlgoo = input.IsOlgoo,

                CustomerCount = data.Count(),
                RecordCount = data.Count(),
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = reportTitle,

                AverageConsumptionPercent = data.Count() > 0 ? data.Average(c => c.PercentConsumptinAverageChange) : 0,
                AverageSumtItemsPercent = data.Count() > 0 ? data.Average(c => c.PercentSumItemsChange) : 0,
            };
            ReportOutput<ConsumptionManagementHeaderOutputDto, ConsumptionManagementDataOutputDto> result = new(reportTitle, header, data);

            return result;
        }
        private string GetQuery()
        {
            return @";WITH BaseBills AS 
						(Select 
							MAX(b.BillId)BillId,
							b.ZoneId,
							b.CustomerNumber,
							MAX(b.ZoneTitle) ZoneTitle,
							SUM(b.SumItems) SumItems,
						    AVG(b.ContractCapacity) ContractCapacity,
							ROUND(AVG(b.ConsumptionAverage),1) ConsumptionAverage,
							SUM(b.Consumption) Consumption, 
							MAX(t.olgo) olgoo
						From CustomerWarehouse.dbo.Bills b
						Join [OldCalc].dbo.table1 t
							ON b.ZoneId=t.town
						Where 
							b.RegisterDay BETWEEN @FromBaseDateJalali AND @ToBaseDateJalali AND
							b.ZoneId IN @zoneIds AND
							(
								(@IsOlgoo=1 AND b.UsageId IN(0,1,3))
								OR
								(@IsOlgoo=0 AND b.UsageId NOT IN(0,1,3))
							) AND
							( 
								b.ConsumptionAverage > @FromMultiplier * IIF(@IsOlgoo=1,t.olgo ,b.ContractCapacity) AND
					            b.ConsumptionAverage <= @ToMultiplier * IIF(@IsOlgoo=1,t.olgo ,b.ContractCapacity)
							) AND
							(
								@FromConsumptionAverage IS NULL OR
								@ToConsumptionAverage IS NULL OR
								b.ConsumptionAverage BETWEEN @FromConsumptionAverage And @ToConsumptionAverage
							) AND
								b.CounterStateCode NOT IN (4,7,8)
						Group By
							b.ZoneId,
							b.CustomerNumber
					),
					ComparisonBills AS 
						(Select 
							MAX(b.BillId)BillId,
							b.ZoneId,
							b.CustomerNumber,
							MAX(b.ZoneTitle) ZoneTitle,
							SUM(b.SumItems) SumItems,
						    AVG(b.ContractCapacity) ContractCapacity,
							ROUND(AVG(b.ConsumptionAverage),1) ConsumptionAverage,
							SUM(b.Consumption) Consumption, 
							MAX(t.olgo) olgoo
						From CustomerWarehouse.dbo.Bills b
						Join [OldCalc].dbo.table1 t
							ON b.ZoneId=t.town
						Where 
							b.RegisterDay BETWEEN @FromComparisonDateJalali AND @ToComparisonDateJalali AND
							b.ZoneId IN @zoneIds  AND
							(
								(@IsOlgoo=1 AND b.UsageId IN(0,1,3))
								OR
								(@IsOlgoo=0 AND b.UsageId NOT IN(0,1,3))
							)
							-- AND
							--( 
							--	b.ConsumptionAverage > @FromMultiplier * IIF(@IsOlgoo=1,t.olgo ,b.ContractCapacity) AND
					  --          b.ConsumptionAverage <= @ToMultiplier * IIF(@IsOlgoo=1,t.olgo ,b.ContractCapacity)
							--) AND
							--(
							--	@FromConsumptionAverage IS NULL OR
							--	@ToConsumptionAverage IS NULL OR
							--	b.ConsumptionAverage BETWEEN @FromConsumptionAverage And @ToConsumptionAverage
							--)	
							AND b.CounterStateCode NOT IN (4,7,8)
						Group By 
							b.ZoneId,
							b.CustomerNumber
					)
					Select 
						b1.BillId,
						b2.BillId,
						b1.ZoneId,
						b1.ZoneTitle,
						b1.CustomerNumber,
						b1.SumItems BaseSumItems,
						IIF(@IsOlgoo=1,b1.olgoo,b1.ContractCapacity) BaseContractOlgoo,
						b1.Consumption BaseSumConsumption,
						b1.ConsumptionAverage BaseConsumptionAverage,
						b2.SumItems ComparisonSumItems,
						IIF(@IsOlgoo=1,b2.olgoo,b2.ContractCapacity) ComparisonContractOlgoo,
						b2.ConsumptionAverage ComparisonConsmptionAverage,
						b2.Consumption ComparisonSumConsumption,
						(
							Case When b1.ConsumptionAverage =0 Then
							-100 Else
							ROUND((b1.ConsumptionAverage-b2.ConsumptionAverage) * 100 / b1.ConsumptionAverage,1)
							End
						) AS PercentConsumptinAverageChange,
					    (
							Case When b1.SumItems=0 Then
							-100 Else 
							ROUND((CAST(b1.SumItems AS float) - CAST( b2.SumItems as float)) * 100.0 /CAST( b1.SumItems as float) ,1)
							End
						) AS PercentSumItemsChange,
						c.ReadingNumber,
						c.FirstName,
						c.SureName SurName,
						(c.FirstName +' '+ c.SureName) FullName,
						c.UsageTitle,
						c.BranchType BranchTypeTitle,
						c.Address,
						c.PhoneNo PhoneNumber,
						c.MobileNo MobileNumber,
						c.WaterDiameterTitle MeterDiameterTitle,
						c.MainSiphonTitle SiphonDiameterTitle
					From BaseBills b1
					Left Join ComparisonBills b2
						ON b1.ZoneId=b2.ZoneId AND b1.CustomerNumber=b2.CustomerNumber
					Left Join CustomerWarehouse.dbo.Clients c
						ON b1.ZoneId=c.ZoneId AND b1.CustomerNumber=c.CustomerNumber
					Where 
						c.ToDayJalali IS NULL ";
        }
    }
}