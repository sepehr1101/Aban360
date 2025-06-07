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

        public async Task<ReportOutput<UnspecifiedWaterPaymentHeaderOutputDto, UnspecifiedWaterPaymentDataOutputDto>> GetInfo(UnspecifiedWaterPaymentInputDto input)
        {
            string unspecifiedWaterPayments = GetUnspecifiedWaterPaymentQuery();
            IEnumerable<UnspecifiedWaterPaymentDataOutputDto> unspecifiedWaterData = await _sqlConnection.QueryAsync<UnspecifiedWaterPaymentDataOutputDto>(unspecifiedWaterPayments);//todo: Parameters
            UnspecifiedWaterPaymentHeaderOutputDto unspecifiedWaterHeader = new UnspecifiedWaterPaymentHeaderOutputDto()
            { };

            var result = new ReportOutput<UnspecifiedWaterPaymentHeaderOutputDto, UnspecifiedWaterPaymentDataOutputDto>(ReportLiterals.UnspecifiedWaterPayment, unspecifiedWaterHeader, unspecifiedWaterData);
            return result;
        }

        private string GetUnspecifiedWaterPaymentQuery()
        {
            return @" ";
        }
    }
}
