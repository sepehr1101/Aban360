using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class WaterPaymentDetailQueryService : AbstractBaseConnection, IWaterPaymentDetailQueryService
    {
        public WaterPaymentDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterPaymentDetailHeaderOutputDto, WaterPaymentDetailDataOutputDto>> GetInfo(WaterPaymentDetailInputDto input)
        {
            string waterPaymentDetails = GetWaterPaymentDetailQuery();
            IEnumerable<WaterPaymentDetailDataOutputDto> waterPaymentDetailDate = await _sqlConnection.QueryAsync<WaterPaymentDetailDataOutputDto>(waterPaymentDetails);//todo: Parameters
            WaterPaymentDetailHeaderOutputDto waterPaymentDetailHeader = new WaterPaymentDetailHeaderOutputDto()
            { };

            var result = new ReportOutput<WaterPaymentDetailHeaderOutputDto, WaterPaymentDetailDataOutputDto>(ReportLiterals.WaterPaymentDetail, waterPaymentDetailHeader, waterPaymentDetailDate);
            return result;
        }

        private string GetWaterPaymentDetailQuery()
        {
            return @" ";
        }
    }
}
