using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class ServiceLinkDailyBankGroupedQueryService : AbstractBaseConnection, IServiceLinkDailyBankGroupedQueryService
    {
        public ServiceLinkDailyBankGroupedQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<DailyBankGroupedHeaderOutputDto, DailyBankGroupedDataOutputDto>> GetInfo(DailyBankGroupedInputDto input)
        {
            string dailyBankGroupeds = GetDailyBankGroupedQuery(input.ZoneIds.Any());
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                input.FromAmount,
                input.ToAmount,
                input.ZoneIds,
                fromBankId=input.FromBankId,
                toBankId=input.ToBankId,
            };
            IEnumerable<DailyBankGroupedDataOutputDto> dailyBankGroupedData = await _sqlReportConnection.QueryAsync<DailyBankGroupedDataOutputDto>(dailyBankGroupeds, @params);
            DailyBankGroupedHeaderOutputDto dailyBankGroupedHeader = new DailyBankGroupedHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromBankId=input.FromBankId,
                ToBankId=input.ToBankId,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = dailyBankGroupedData is not null && dailyBankGroupedData.Any() ? dailyBankGroupedData.Count() : 0,
                CustomerCount = dailyBankGroupedData is not null && dailyBankGroupedData.Any() ? dailyBankGroupedData.Count() : 0,

                TotalCount = dailyBankGroupedData?.Sum(r => r.TotalCount) ?? 0,
                TotalAmount = dailyBankGroupedData?.Sum(r => r.TotalAmount) ?? 0,
            };

            var result = new ReportOutput<DailyBankGroupedHeaderOutputDto, DailyBankGroupedDataOutputDto>(ReportLiterals.SewageDailyBankGrouped, dailyBankGroupedHeader, dailyBankGroupedData);
            return result;
        }

        private string GetDailyBankGroupedQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND p.ZoneId IN @ZoneIds" : string.Empty;

            return @$"Select 
                    	p.RegisterDay AS RegisterDate, 
                    	p.PayDateJalali AS BankDate,
                        p.ZoneTitle AS ZoneTitle,
                        p.BankName AS BankName,
                        p.BankCode AS BankCode,
                    	COUNT(p.RegisterDay) AS ItemCount,
                    	SUM(p.Amount) AS ItemAmount,
                    	COUNT(p.RegisterDay) AS TotalCount,
                    	SUM(p.Amount) AS TotalAmount
                    From [CustomerWarehouse].dbo.PaymentsEn p
                    WHERE 
                    	(
                            (@FromDate IS NOT NULL AND @ToDate IS NOT NULL AND p.RegisterDay BETWEEN @FromDate AND @ToDate)
                            OR (@FromDate IS NULL AND @ToDate IS NULL)
                        )
                        AND 
                        (
                            (@FromAmount IS NOT NULL AND @ToAmount IS NOT NULL AND p.Amount BETWEEN @FromAmount AND @ToAmount)
                            OR (@FromAmount IS NULL AND @ToAmount IS NULL)
                        )AND
						(@fromBankId IS NULL OR
						@toBankId IS NULL OR
						p.BankCode BETWEEN @fromBankId AND @toBankId)
                    {zoneQuery}
                    GROUP BY p.RegisterDay,
							 p.PayDateJalali,
                             p.BankName,   
                             p.BankCode,
                             p.ZoneTitle";
        }
    }
}
