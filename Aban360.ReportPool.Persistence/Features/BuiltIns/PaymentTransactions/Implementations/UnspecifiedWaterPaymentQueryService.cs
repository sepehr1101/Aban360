using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
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
            IEnumerable<UnspecifiedPaymentDataOutputDto> unspecifiedWaterData = await _sqlConnection.QueryAsync<UnspecifiedPaymentDataOutputDto>(unspecifiedWaterPayments);//todo: Parameters
            UnspecifiedPaymentHeaderOutputDto unspecifiedWaterHeader = new UnspecifiedPaymentHeaderOutputDto()
            { };

            var result = new ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto>(ReportLiterals.UnspecifiedWaterPayment, unspecifiedWaterHeader, unspecifiedWaterData);
            return result;
        }

        private string GetUnspecifiedWaterPaymentQuery()
        {
            return @" ";
        }
    }
}
