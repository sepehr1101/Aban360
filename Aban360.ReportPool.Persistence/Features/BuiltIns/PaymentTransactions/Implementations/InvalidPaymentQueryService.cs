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
    internal sealed class InvalidPaymentQueryService : AbstractBaseConnection, IInvalidPaymentQueryService
    {
        public InvalidPaymentQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<InvalidPaymentHeaderOutputDto, InvalidPaymentDataOutputDto>> GetInfo(InvalidPaymentInputDto input)
        {
            string invalidPayments = GetInvalidPaymentQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
            };
            IEnumerable<InvalidPaymentDataOutputDto> invalidPaymentData = await _sqlReportConnection.QueryAsync<InvalidPaymentDataOutputDto>(invalidPayments, @params);
            InvalidPaymentHeaderOutputDto invalidPaymentHeader = new InvalidPaymentHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (invalidPaymentData is not null && invalidPaymentData.Any()) ? invalidPaymentData.Count() : 0,
                TotalAmount = invalidPaymentData.Sum(x => x.Amount),
            };

            var result = new ReportOutput<InvalidPaymentHeaderOutputDto, InvalidPaymentDataOutputDto>(ReportLiterals.InvalidPayment, invalidPaymentHeader, invalidPaymentData);
            return result;
        }

        private string GetInvalidPaymentQuery()
        {
            return @"Select
                    	i.CustomerNumber,
                    	i.BillId,
                    	i.ZoneId,
                        i.ZoneTitle,
                    	i.BankDateJalali,
                    	i.PayId,
                    	i.BankAbbriviation,
                    	i.BankCode,
                    	i.CheckState,
                        i.Amount
                    From [vosoli].dbo.V_InvalidCredit i
                    Where 
                    	i.RegisterDateJalali BETWEEN @fromDate AND @toDate ";
        }
    }
}
