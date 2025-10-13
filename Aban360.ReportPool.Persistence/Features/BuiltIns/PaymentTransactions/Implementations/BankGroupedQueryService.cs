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
    internal sealed class BankGroupedQueryService : AbstractBaseConnection, IBankGroupedQueryService
    {
        public BankGroupedQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<BankGroupedHeaderOutputDto, BankGroupedDataOutputDto>> GetInfo(BankGroupedInputDto input)
        {
            string BankGroupeds = GetBankGroupedQuery();
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                fromBankId = input.FromBankId,
                toBankId = input.ToBankId,
            };
            IEnumerable<BankGroupedDataOutputDto> BankGroupedData = await _sqlReportConnection.QueryAsync<BankGroupedDataOutputDto>(BankGroupeds, @params);
            BankGroupedHeaderOutputDto BankGroupedHeader = new BankGroupedHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromBankId = input.FromBankId,
                ToBankId = input.ToBankId,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = BankGroupedData is not null && BankGroupedData.Any() ? BankGroupedData.Count() : 0,
                CustomerCount = BankGroupedData is not null && BankGroupedData.Any() ? BankGroupedData.Count() : 0,
                Title= ReportLiterals.BankGrouped,

                TotalAmount = BankGroupedData?.Sum(r => r.TotalAmount) ?? 0,
                TotalCount = BankGroupedData?.Sum(r => r.TotalCount) ?? 0,
                WaterCount = BankGroupedData?.Sum(r => r.WaterCount) ?? 0,
                WaterAmount = BankGroupedData?.Sum(r => r.WaterAmount) ?? 0,
                ServiceLinkAmount = BankGroupedData?.Sum(r => r.ServiceLinkAmount) ?? 0,
                ServiceLinkCount = BankGroupedData?.Sum(r => r.ServiceLinkCount) ?? 0,

            };

            var result = new ReportOutput<BankGroupedHeaderOutputDto, BankGroupedDataOutputDto>(ReportLiterals.BankGrouped, BankGroupedHeader, BankGroupedData);
            return result;
        }

        private string GetBankGroupedQuery()
        {
            return @$"with Payment as(
					Select 
                        p.BankName AS BankName,
                        p.BankCode AS BankCode,
                    	SUM(p.Amount) AS WaterAmount,
						COUNT(p.Amount) AS WaterCount
                    From [CustomerWarehouse].dbo.Payments p
                    WHERE 
                    	(
                            (@FromDate IS NOT NULL AND @ToDate IS NOT NULL AND p.RegisterDay BETWEEN @FromDate AND @ToDate)
                            OR (@FromDate IS NULL AND @ToDate IS NULL)
                        )AND
						(@fromBankId IS NULL OR
						@toBankId IS NULL OR
						p.BankCode BETWEEN @fromBankId AND @toBankId)
                    GROUP BY p.BankName,
                             p.BankCode
					),
					 PaymentEn as(
					Select 
                        p.BankName AS BankName,
                        p.BankCode AS BankCode,
                    	SUM(p.Amount) AS ServiceLinkAmount,
						COUNT(p.Amount) AS ServiceLinkCount
                    From [CustomerWarehouse].dbo.PaymentsEn p
                    WHERE 
                    	(
                            (@FromDate IS NOT NULL AND @ToDate IS NOT NULL AND p.RegisterDay BETWEEN @FromDate AND @ToDate)
                            OR (@FromDate IS NULL AND @ToDate IS NULL)
                        )AND
						(@fromBankId IS NULL OR
						@toBankId IS NULL OR
						p.BankCode BETWEEN @fromBankId AND @toBankId)
                    GROUP BY p.BankName,
                             p.BankCode
					)
				Select 
					p.BankName,
					p.BankCode,
					p.WaterAmount,
					p.WaterCount,
					pe.ServiceLinkAmount,
					pe.ServiceLinkCount,
					p.WaterAmount + pe.ServiceLinkAmount AS TotalAmount,
					p.WaterCount + pe.ServiceLinkCount AS TotalCount
				From Payment p
				Join PaymentEn pe
					On pe.BankCode=p.BankCode";
        }
    }
}
