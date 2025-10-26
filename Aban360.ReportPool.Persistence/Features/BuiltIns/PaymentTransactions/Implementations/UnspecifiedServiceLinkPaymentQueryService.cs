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
    internal sealed class UnspecifiedServiceLinkPaymentQueryService : AbstractBaseConnection, IUnspecifiedServiceLinkPaymentQueryService
    {
        public UnspecifiedServiceLinkPaymentQueryService(IConfiguration configuration)
            : base(configuration)
        { 
		}

        public async Task<ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto>> GetInfo(UnspecifiedPaymentInputDto input)
        {
            string unspecifiedServiceLinkPayments = GetUnspecifiedServiceLinkPaymentQuery();

            IEnumerable<UnspecifiedPaymentDataOutputDto> unspecifiedServiceLinkData = await _sqlReportConnection.QueryAsync<UnspecifiedPaymentDataOutputDto>(unspecifiedServiceLinkPayments,input,null,180);
			UnspecifiedPaymentHeaderOutputDto unspecifiedServiceLinkHeader = new UnspecifiedPaymentHeaderOutputDto()
			{
				FromDateJalali = input.FromDateJalali,
				ToDateJalali = input.ToDateJalali,
				FromAmount = input.FromAmount,
				ToAmount = input.ToAmount,
                FromBankId = input.FromBankId,
                ToBankId = input.ToBankId,
                CustomerCount = (unspecifiedServiceLinkData is not null && unspecifiedServiceLinkData.Any()) ? unspecifiedServiceLinkData.Count() : 0,
				RecordCount = (unspecifiedServiceLinkData is not null && unspecifiedServiceLinkData.Any()) ? unspecifiedServiceLinkData.Count() : 0,
                TotalAmount = unspecifiedServiceLinkData.Sum(serviceLink => serviceLink.Amount),
				FileName = "-",
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
				Title= ReportLiterals.UnspecifiedServiceLinkPayment,
            };

            var result = new ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto>(ReportLiterals.UnspecifiedServiceLinkPayment, unspecifiedServiceLinkHeader, unspecifiedServiceLinkData);
            return result;
        }

        private string GetUnspecifiedServiceLinkPaymentQuery()
        {
            return @$"Select TOP 1000
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
					From [CustomerWarehouse].dbo.PaymentsEn p
					LEFT JOIN [CustomerWarehouse].dbo.RequestBillDetails b 
						ON p.PayId=b.PayId AND p.ZoneId=b.ZoneId AND p.CustomerNumber=b.CustomerNumber
					WHERE
						b.Id IS NULL AND
						(
							@FromDateJalali IS NULL OR
							@ToDateJalali IS NULL OR
							p.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali
						) AND
						(
							@FromAmount IS  NULL OR
							@ToAmount IS NULL OR
							p.Amount BETWEEN @FromAmount AND @ToAmount
						) AND
						(
							@fromBankId IS NULL OR
							@toBankId IS NULL OR
							p.BankCode BETWEEN @fromBankId AND @toBankId
						)";
        }
    }
}
