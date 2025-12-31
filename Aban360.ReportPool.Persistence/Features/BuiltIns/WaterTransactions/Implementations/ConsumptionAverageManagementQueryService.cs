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
    internal sealed class ConsumptionAverageManagementQueryService : AbstractBaseConnection, IConsumptionAverageManagementQueryService
    {
        public ConsumptionAverageManagementQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementDataOutputDto>> Get(ConsumptionAverageManagementInputDto input)
        {
            string reportTitle = ReportLiterals.ConsumptionManagerDetail + ReportLiterals.ByBill;
            string query = GetQuery();
            IEnumerable<ConsumptionAverageManagementDataOutputDto> data = await _sqlReportConnection.QueryAsync<ConsumptionAverageManagementDataOutputDto>(query, input, null, 600);
            ConsumptionAverageManagementHeaderOutputDto header = new ConsumptionAverageManagementHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                CustomerCount = data.Count(),
                RecordCount = data.Count(),
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = reportTitle,

                Consumption = data.Count() > 0 ? data.Average(c => c.Consumption) : 0,
                ConsumptionAverage = data.Count() > 0 ? data.Average(c => c.ConsumptionAverage) : 0,
            };
            ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementDataOutputDto> result = new(reportTitle, header, data);

            return result;
        }
        private string GetQuery()
        {
            return @"Select 
                    	b.ZoneId,
                    	b.ZoneTitle,
                    	b.CustomerNumber,
                        b.BillId,
                    	b.Consumption,
                    	b.ConsumptionAverage,
                    	b.RegisterDay as RegisterDayJalali,
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
                    	c.MainSiphonTitle SiphonDiameterTitle,
                        c.DomesticCount AS DomesticUnit,
                        c.CommercialCount AS CommercialUnit,
                        c.OtherCount AS OtherUnit,
                        IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount)) AS TotalUnit
                    From CustomerWarehouse.dbo.Bills b
                    Left Join CustomerWarehouse.dbo.Clients c
                    	On b.ZoneId=c.ZoneId AND b.CustomerNumber= c.CustomerNumber
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	b.ZoneId IN @zoneIds AND
                    	b.UsageId IN @usageIds";
        }
    }
}
