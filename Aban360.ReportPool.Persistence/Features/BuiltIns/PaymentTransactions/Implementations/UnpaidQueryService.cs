using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class UnpaidQueryService : AbstractBaseConnection, IUnpaidQueryService
    {
        public UnpaidQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<UnpaidHeaderOutputDto, UnpaidDataOutputDto>> GetInfo(UnpaidInputDto input)
        {
            string unpaids = GetUnpaidQuery(input.ZoneIds?.Any() == true);
            var @params = new
            {
                FromAmount = input.FromAmount ?? 0,
                ToAmount = input.ToAmount ?? long.MaxValue,
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ZoneIds = input.ZoneIds,
            };
            IEnumerable<UnpaidDataOutputDto> unpaidData = await _sqlReportConnection.QueryAsync<UnpaidDataOutputDto>(unpaids, @params);
            UnpaidHeaderOutputDto unpaidHeader = new UnpaidHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                CustomerCount = (unpaidData is not null && unpaidData.Any()) ? unpaidData.Count() : 0,
                RecordCount = (unpaidData is not null && unpaidData.Any()) ? unpaidData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                DebtAmount = unpaidData?.Sum(r => r.DebtAmount) ?? 0,
                Title= ReportLiterals.Unpaid,
            };

            var result = new ReportOutput<UnpaidHeaderOutputDto, UnpaidDataOutputDto>(ReportLiterals.Unpaid, unpaidHeader, unpaidData);
            return result;
        }

        private string GetUnpaidQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId IN @ZoneIds" : string.Empty;

            return @$"SELECT 
                        MAX(b.ZoneId) AS ZoneId,
						MAX(b.ZoneTitle) AS ZoneTitle,
                    	MAX(b.CustomerNumber) AS CustomerNumber,
                    	MAX(b.ReadingNumber) AS ReadingNumber,
                    	(max(c.FirstName) +' '+max(c.SureName)) AS FullName,
                    	 MAX(b.WaterDiameterTitle) AS MeterDiameterTitle,
                    	 MAX(b.UsageTitle) AS UsageSellTitle,
                    	 MAX(b.SumItems) AS DebtAmount,
                    	max(c.Address) AS Address ,
                    	COUNT(b.BillId) AS PeriodCount,
                    	MAX(b.BillId) AS BillId
                    FROM [CustomerWarehouse].dbo.bills as b
                    LEFT JOIN [CustomerWarehouse].dbo.payments as p ON p.BillTableId = b.id
					join [CustomerWarehouse].dbo.Clients c on b.BillId=c.BillId
                    WHERE 
                    p.id IS NULL
                    AND (b.RegisterDay BETWEEN @FromDate and @ToDate)
                    AND (@FromAmount is null or
                    	 @ToAmount is null or 
                    	 b.Payable BETWEEN @FromAmount and @ToAmount)
                    AND (@FromReadingNumber is null or
                    	 @ToReadingNumber is null or 
                    	 c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber)
            		AND c.ToDayJalali IS NULL                    
                    {zoneQuery}
                    group by b.BillId";

        }
    }
}
