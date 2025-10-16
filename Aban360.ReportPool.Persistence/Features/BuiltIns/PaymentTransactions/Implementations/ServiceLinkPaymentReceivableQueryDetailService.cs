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
    internal sealed class ServiceLinkPaymentReceivableQueryDetailService : PaymentReceivableBase, IServiceLinkPaymentReceivableQueryDetailService
    {
        public ServiceLinkPaymentReceivableQueryDetailService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>> GetInfo(ServiceLinkPaymentReceivableInputDto input)
        {
            string query = GetDetailQuery(false, input.ZoneIds.HasValue(), input.UsageIds.HasValue());

            IEnumerable<WaterPaymentReceivableDataOutputDto> waterPaymentReceivableData = await _sqlReportConnection.QueryAsync<WaterPaymentReceivableDataOutputDto>(query, input);
            WaterPaymentReceivableHeaderOutputDto waterPaymentReceivableHeader = new WaterPaymentReceivableHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = waterPaymentReceivableData is not null && waterPaymentReceivableData.Any() ? waterPaymentReceivableData.Count() : 0,
                CustomerCount = waterPaymentReceivableData is not null && waterPaymentReceivableData.Any() ? waterPaymentReceivableData.Count() : 0,
                BillCount = waterPaymentReceivableData is not null && waterPaymentReceivableData.Any() ? waterPaymentReceivableData.Count() : 0,
                Title= ReportLiterals.ServiceLinkPaymentReceivableDetail,

            };
            var result = new ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>(ReportLiterals.ServiceLinkPaymentReceivableDetail, waterPaymentReceivableHeader, waterPaymentReceivableData);
            return result;
        }
    }
}
