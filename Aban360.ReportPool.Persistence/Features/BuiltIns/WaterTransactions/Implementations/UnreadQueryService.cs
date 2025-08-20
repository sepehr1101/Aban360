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
                PeriodCount= input.PeriodCount,
                ZoneIds=input.ZoneIds,
            };
            IEnumerable<UnreadDataOutputDto> unreadData = await _sqlReportConnection.QueryAsync<UnreadDataOutputDto>(unread,@params);
            UnreadHeaderOutputDto unreadHeader = new UnreadHeaderOutputDto()
            { 
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber= input.ToReadingNumber,
                PeriodCount= input.PeriodCount,
                ReportDateJalali=DateTime.Now.ToShortPersianDateString(),
                RecordCount= (unreadData is not null && unreadData.Any()) ? unreadData.Count() : 0,
            };

            var result = new ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto>(ReportLiterals.Unread, unreadHeader, unreadData);
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
                    	MAX(c.Address) AS Address,
                    	COUNT(b.BillId) AS PeriodCount,
                        MAX(b.ZoneTitle) AS ZoneTitle
                    From [CustomerWarehouse].dbo.Bills b
                    JOIN [CustomerWarehouse].dbo.Clients c ON b.BillId=c.BillId
                    LEFT JOIN [CustomerWarehouse].dbo.payments as p ON p.BillTableId = b.id
                    WHERE
            			c.ToDayJalali IS NULL AND
                    	p.id IS NULL AND
                        b.TypeId=N'بسته مانع' AND
                         (
                         	(@FromReadingNumber IS NOT NULL AND
                         		@ToReadingNumber IS NOT NULL AND
                         		c.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber)
                         	OR
                         	(@FromReadingNumber IS NULL AND
                         		@ToReadingNumber IS NULL)
                         )
                        {zoneQuery}
                    GROUP BY b.BillId
                    HAVING COUNT(b.BillId)=@PeriodCount";
        }
    }
}
