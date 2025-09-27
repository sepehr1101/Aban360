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
    internal sealed class UnspecifiedWaterPaymentQueryService : AbstractBaseConnection, IUnspecifiedWaterPaymentQueryService
    {
        public UnspecifiedWaterPaymentQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto>> GetInfo(UnspecifiedPaymentInputDto input)
        {
            string unspecifiedWaterPayments = GetUnspecifiedWaterPaymentQuery();
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromBankId = input.FromBankId,
                ToBankId = input.ToBankId,
            };
            IEnumerable<UnspecifiedPaymentDataOutputDto> unspecifiedWaterData = await _sqlReportConnection.QueryAsync<UnspecifiedPaymentDataOutputDto>(unspecifiedWaterPayments,@params);
            UnspecifiedPaymentHeaderOutputDto unspecifiedWaterHeader = new UnspecifiedPaymentHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                CustomerCount = (unspecifiedWaterData is not null && unspecifiedWaterData.Any()) ? unspecifiedWaterData.Count() : 0,
                RecordCount = (unspecifiedWaterData is not null && unspecifiedWaterData.Any()) ? unspecifiedWaterData.Count() : 0,
                TotalAmount = unspecifiedWaterData.Sum(serviceLink => serviceLink.Amount),
                FileName = "-",
                ReportDateJalali = DateTime.Now.ToShortPersianDateString()
            };


            var result = new ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto>(ReportLiterals.UnspecifiedWaterPayment, unspecifiedWaterHeader, unspecifiedWaterData);
            return result;
        }
        private string GetUnspecifiedWaterPaymentQuery()
        {
            return @$"Select
						p.CustomerNumber AS CustomerNumber,
						p.RegisterDay AS EventDateJalali,
						p.RegisterDay AS BankDateJalali,
						p.BankName AS BankName,
						p.BankCode AS BankId,
						p.BillId AS BillId,
						p.PayId AS PaymentId,
						p.RegisterDay AS PaymentDateJalali,
						p.Amount AS Amount,
						p.PaymentGateway AS PaymentGateway
					From [CustomerWarehouse].dbo.Payments p
					LEFT JOIN [CustomerWarehouse].dbo.Bills b 
						ON p.PayId=b.PayId
					WHERE
						b.Id IS NULL
						AND
						(
							(@FromDate IS NOT NULL AND
								@ToDate IS NOT NULL AND
								p.RegisterDay BETWEEN @FromDate AND @ToDate)
							OR
							(@FromDate IS NULL AND
								@ToDate IS NULL)
						)
						AND
						(
							(@FromAmount IS NOT NULL AND
							  @ToAmount IS NOT NULL AND
							  p.Amount BETWEEN @FromAmount AND @ToAmount)
							OR 
							(@FromAmount IS NULL AND
								@ToAmount IS NULL)
						)AND
						(p.BankCode BETWEEN @FromBankId AND @ToBankId)";
        }
    }
}
