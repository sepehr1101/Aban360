using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class UnspecifiedServiceLinkPaymentQueryService : AbstractBaseConnection, IUnspecifiedServiceLinkPaymentQueryService
    {
        public UnspecifiedServiceLinkPaymentQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto>> GetInfo(UnspecifiedPaymentInputDto input)
        {
            string unspecifiedServiceLinkPayments = GetUnspecifiedServiceLinkPaymentQuery();
            IEnumerable<UnspecifiedPaymentDataOutputDto> unspecifiedServiceLinkData = await _sqlConnection.QueryAsync<UnspecifiedPaymentDataOutputDto>(unspecifiedServiceLinkPayments);//todo: Parameters
            UnspecifiedPaymentHeaderOutputDto unspecifiedServiceLinkHeader = new UnspecifiedPaymentHeaderOutputDto()
            { };

            var result = new ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto>(ReportLiterals.UnspecifiedServiceLinkPayment, unspecifiedServiceLinkHeader, unspecifiedServiceLinkData);
            return result;
        }

        private string GetUnspecifiedServiceLinkPaymentQuery()
        {
            return @" ";
        }
    }
}
