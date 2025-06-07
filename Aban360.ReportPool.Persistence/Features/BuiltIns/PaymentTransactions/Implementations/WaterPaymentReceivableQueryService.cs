using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class WaterPaymentReceivableQueryService : AbstractBaseConnection, IWaterPaymentReceivableQueryService
    {
        public WaterPaymentReceivableQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>> GetInfo(WaterPaymentReceivableInputDto input)
        {
            string paymentReceivables = GetWaterPaymentReceivableQuery();
            IEnumerable<WaterPaymentReceivableDataOutputDto> waterPaymentReceivableDate = await _sqlConnection.QueryAsync<WaterPaymentReceivableDataOutputDto>(paymentReceivables);//todo: Parameters
            WaterPaymentReceivableHeaderOutputDto waterPaymentReceivableHeader = new WaterPaymentReceivableHeaderOutputDto()
            { };

            var result = new ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>(ReportLiterals.WaterPaymentReceivable, waterPaymentReceivableHeader, waterPaymentReceivableDate);
            return result;
        }

        private string GetWaterPaymentReceivableQuery()
        {
            return @" ";
        }
    }
}
