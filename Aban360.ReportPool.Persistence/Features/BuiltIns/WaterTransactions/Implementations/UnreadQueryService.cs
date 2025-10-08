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
    internal sealed class UnreadQueryService : UnreadBase, IUnreadQueryService
    {
        public UnreadQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto>> GetInfo(UnreadInputDto input)
        {
            string query = GetDetailQuery(input.ZoneIds?.Any()==true);
            //string query = GetUnreadQuery(input.ZoneIds?.Any()==true);

            var @params = new
            { 
                FromReadingNumber= input.FromReadingNumber,
                ToReadingNumber= input.ToReadingNumber,
                FromPeriodCount= input.FromPeriodCount,
                ToPeriodCount= input.ToPeriodCount,
                ZoneIds=input.ZoneIds,
            };
            IEnumerable<UnreadDataOutputDto> unreadData = await _sqlReportConnection.QueryAsync<UnreadDataOutputDto>(query,@params);
            UnreadHeaderOutputDto unreadHeader = new UnreadHeaderOutputDto()
            { 
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber= input.ToReadingNumber,
                FromPeriodCount= input.FromPeriodCount,
                ToPeriodCount= input.ToPeriodCount,
                ReportDateJalali=DateTime.Now.ToShortPersianDateString(),
                RecordCount= (unreadData is not null && unreadData.Any()) ? unreadData.Count() : 0,
                CustomerCount = (unreadData is not null && unreadData.Any()) ? unreadData.Count() : 0,
            };

            var result = new ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto>(ReportLiterals.UnreadDetail, unreadHeader, unreadData);
            return result;
        }

        private string GetUnreadQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId in @ZoneIds" : string.Empty;

            return @$";WITH LatestBill AS (
                    SELECT
                        TRIM(b.BillId) AS BillId,
                		b.CustomerNumber,
                		b.ReadingNumber,
                		b.CounterStateTitle,
                		b.ZoneTitle,
                		b.ZoneId,
                        b.TypeId,
                		b.SumItems,
                        ROW_NUMBER() OVER (PARTITION BY TRIM(b.BillId) ORDER BY b.RegisterDay DESC) AS RN
                    FROM [CustomerWarehouse].dbo.Bills b
                    WHERE 
                        (@FromReadingNumber IS NULL OR
                         @ToReadingNumber IS NULL OR
                         b.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber)
                        {zoneQuery}
                )
                    SELECT 
                		L.BillId,
                		MAX(L.CustomerNumber)as CustomerNumber,
                		MAX(L.ReadingNumber)as ReadingNumber,
                		MAX(L.CounterStateTitle)as CounterStateTitle,
                		MAX(L.ZoneTitle)as ZoneTitle,
                        MAX(L.TypeId)as TypeId,
                		MAX(L.SumItems)as SumItems,
                		MAX(TRIM(c.FirstName)) +' '+MAX(TRIM(c.Surename)) AS FullName,
                		MAX(TRIM(c.FatherName)) as FatherName,
                		MAX(c.WaterDiameterTitle) AS MeterDiameterTitle,
                		MAX(c.UsageTitle) AS UsageSellTitle,
                		MAX(TRIM(c.Address)) AS Address,
                		MAX(c.WaterRequestDate)  AS MeterRequestDateJalali,
                		MAX(c.WaterRegisterDateJalali) AS MeterInstallationDateJalali,
                		MAX(TRIM(c.MobileNo)) as MobileNumber,
                		MAX(TRIM(c.PhoneNo)) as PhoneNumber,
                		MAX(c.ContractCapacity) as ContractualCapacity,
                		MAX(c.CommercialCount) as CommercialUnit,
                		MAX(c.DomesticCount) as DomesticUnit,
                		MAX(c.OtherCount) as OtherUnit,
                		MAX(c.ContractCapacity) as ContractualCapacity,
                		MAX(c.UsageTitle) as UsageTitle,
                        MAX(c.MainSiphonTitle) as SiphonDiameterTitle,
                        MAX(c.EmptyCount) as EmptyUnit,
                		MAX(TRIM(c.NationalId)) as NationalCode
                    FROM LatestBill l
                	Left Join [CustomerWarehouse].dbo.Clients c
                		On l.ZoneId=c.ZoneId AND l.CustomerNumber=c.CustomerNumber
                    WHERE
                		(l.RN BETWEEN @FromPeriodCount AND @ToPeriodCount) AND 
                		c.ToDayJalali IS NULL
                    GROUP BY l.BillId
                    HAVING COUNT(CASE WHEN l.TypeId = N'بسته مانع' THEN 1 END) >= @ToPeriodCount";
        }
    }
}
