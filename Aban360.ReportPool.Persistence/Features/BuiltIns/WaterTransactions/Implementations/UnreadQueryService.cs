using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class UnreadQueryService : AbstractBaseConnection, IUnreadQueryService
    {
        public UnreadQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto>> GetInfo(UnreadInputDto input)
        {
            string unread = GetUnreadQuery(input.ZoneIds?.Any()==true);
            var @params = new
            { 
                FromReadingNumber= input.FromReadingNumber,
                ToReadingNumber= input.ToReadingNumber,
                FromPeriodCount= input.FromPeriodCount,
                ToPeriodCount= input.ToPeriodCount,
                ZoneIds=input.ZoneIds,
            };
            IEnumerable<UnreadDataOutputDto> unreadData = await _sqlReportConnection.QueryAsync<UnreadDataOutputDto>(unread,@params);
            UnreadHeaderOutputDto unreadHeader = new UnreadHeaderOutputDto()
            { 
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber= input.ToReadingNumber,
                FromPeriodCount= input.FromPeriodCount,
                ToPeriodCount= input.ToPeriodCount,
                ReportDateJalali=DateTime.Now.ToShortPersianDateString(),
                RecordCount= (unreadData is not null && unreadData.Any()) ? unreadData.Count() : 0,
            };

            var result = new ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto>(ReportLiterals.UnreadDetail, unreadHeader, unreadData);
            return result;
        }

        private string GetUnreadQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId in @ZoneIds" : string.Empty;

            return @$"Select 
                    	b.BillId AS BillId,
                    	MAX(b.CustomerNumber) AS CustomerNumber,
                    	MAX(b.ReadingNumber) AS ReadingNumber,
                    	MAX(c.FirstName) +' '+MAX(c.Surename) AS FullName,
                    	MAX(c.WaterDiameterTitle) AS MeterDiameterTitle,
                    	MAX(c.UsageTitle2) AS UsageSellTitle,
                    	SUM(DISTINCT b.Payable) - SUM(DISTINCT p.Amount) AS DebtAmount,
                    	MAX(TRIM(c.Address)) AS Address,
                    	COUNT(b.BillId) AS PeriodCount,
                        MAX(b.ZoneTitle) AS ZoneTitle,
                        MAX(b.CounterStateTitle) AS CounterStateTitle,
                        MAX(c.WaterRequestDate)  AS MeterRequestDateJalali,
						MAX(c.WaterRegisterDateJalali) AS MeterInstallationDateJalali,
						MAX(TRIM(c.MobileNo)) as MobileNumber,
						MAX(TRIM(c.PhoneNo)) as PhoneNumber,
						MAX(c.ContractCapacity) as ContractualCapacity,
						MAX(c.CommercialCount) as CommercialUnit,
						MAX(c.DomesticCount) as DomesticUnit,
						MAX(c.OtherCount) as OtherUnit,
						(MAX(c.ContractCapacity) + MAX(c.DomesticCount) + MAX(c.OtherCount)) as TotalUnit,
						MAX(CAST(c.HasCommonSiphon as int)) as SiphonDiameterTitle,
						MAX(c.UsageTitle) as UsageTitle,
						MAX(TRIM(c.NationalId)) as NationalCode,
						MAX(c.EmptyCount) as EmptyUnit
                    From [CustomerWarehouse].dbo.Bills b
                    JOIN [CustomerWarehouse].dbo.Clients c ON b.BillId=c.BillId
                    LEFT JOIN [CustomerWarehouse].dbo.payments as p ON p.BillTableId = b.id
                    WHERE
            			c.ToDayJalali IS NULL AND
                    	p.id IS NULL AND
                        b.TypeId=N'بسته مانع' AND
                         	(@FromReadingNumber IS NULL OR
                         	@ToReadingNumber IS NULL OR
                         	c.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber)
                        {zoneQuery}
                    GROUP BY b.BillId
                    HAVING COUNT(b.BillId) BETWEEN @FromPeriodCount AND @ToPeriodCount";
        }
    }
}
