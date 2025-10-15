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
    internal sealed class WaterPaymentReceivableQueryDetailService : PaymentReceivableBase, IWaterPaymentReceivableQueryDetailService
    {
        public WaterPaymentReceivableQueryDetailService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>> GetInfo(WaterPaymentReceivableInputDto input)
        {
            string query = GetDetailQuery(true,input.ZoneIds.HasValue());
            
            IEnumerable<WaterPaymentReceivableDataOutputDto> waterPaymentReceivableData = await _sqlReportConnection.QueryAsync<WaterPaymentReceivableDataOutputDto>(query, input);
            WaterPaymentReceivableHeaderOutputDto waterPaymentReceivableHeader = new WaterPaymentReceivableHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title= ReportLiterals.WaterPaymentReceivableDetail,
            };
            if (waterPaymentReceivableData is not null && waterPaymentReceivableData.Any())
            {
                waterPaymentReceivableHeader.RecordCount = (waterPaymentReceivableData is not null && waterPaymentReceivableData.Any()) ? waterPaymentReceivableData.Count() : 0;  
                waterPaymentReceivableHeader.CustomerCount = (waterPaymentReceivableData is not null && waterPaymentReceivableData.Any()) ? waterPaymentReceivableData.Count() : 0;  
                waterPaymentReceivableHeader.Amount = waterPaymentReceivableData?.Sum(r=>r.Amount) ?? 0;                
            }
            var result = new ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>(ReportLiterals.WaterPaymentReceivableDetail, waterPaymentReceivableHeader, waterPaymentReceivableData);
            return result;
        }
    }
}
