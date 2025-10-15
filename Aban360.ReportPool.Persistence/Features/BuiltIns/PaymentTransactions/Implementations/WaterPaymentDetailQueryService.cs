using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
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
    internal sealed class WaterPaymentDetailQueryService : PaymentBase, IWaterPaymentDetailQueryService
    {
        public WaterPaymentDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>> GetInfo(PaymentDetailInputDto input)
        {
            string query = GetDetailQuery(true, input.ZoneIds.HasValue());
           
            IEnumerable<PaymentDetailDataOutputDto> waterPaymentDetailData = await _sqlReportConnection.QueryAsync<PaymentDetailDataOutputDto>(query, input);
            PaymentDetailHeaderOutputDto waterPaymentDetailHeader = new PaymentDetailHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                ReportDateJalali=DateTime.Now.ToShortPersianDateString(),
                RecordCount = (waterPaymentDetailData is not null && waterPaymentDetailData.Any()) ? waterPaymentDetailData.Count() : 0,
                CustomerCount = (waterPaymentDetailData is not null && waterPaymentDetailData.Any()) ? waterPaymentDetailData.Count() : 0,
                TotalAmount = waterPaymentDetailData.Sum(payment => Convert.ToInt64(payment.Amount)),
                Title= ReportLiterals.WaterPaymentDetail,
            };

            var result = new ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>(ReportLiterals.WaterPaymentDetail, waterPaymentDetailHeader, waterPaymentDetailData);
            return result;
        }
    }
}
