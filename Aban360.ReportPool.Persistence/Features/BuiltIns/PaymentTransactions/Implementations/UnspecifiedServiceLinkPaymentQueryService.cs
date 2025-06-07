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

        public async Task<ReportOutput<UnspecifiedServiceLinkPaymentHeaderOutputDto, UnspecifiedServiceLinkPaymentDataOutputDto>> GetInfo(UnspecifiedServiceLinkPaymentInputDto input)
        {
            string unspecifiedServiceLinkPayments = GetUnspecifiedServiceLinkPaymentQuery();
            IEnumerable<UnspecifiedServiceLinkPaymentDataOutputDto> unspecifiedServiceLinkData = await _sqlConnection.QueryAsync<UnspecifiedServiceLinkPaymentDataOutputDto>(unspecifiedServiceLinkPayments);//todo: Parameters
            UnspecifiedServiceLinkPaymentHeaderOutputDto unspecifiedServiceLinkHeader = new UnspecifiedServiceLinkPaymentHeaderOutputDto()
            { };

            var result = new ReportOutput<UnspecifiedServiceLinkPaymentHeaderOutputDto, UnspecifiedServiceLinkPaymentDataOutputDto>(ReportLiterals.UnspecifiedServiceLinkPayment, unspecifiedServiceLinkHeader, unspecifiedServiceLinkData);
            return result;
        }

        private string GetUnspecifiedServiceLinkPaymentQuery()
        {
            return @" ";
        }
    }
}
