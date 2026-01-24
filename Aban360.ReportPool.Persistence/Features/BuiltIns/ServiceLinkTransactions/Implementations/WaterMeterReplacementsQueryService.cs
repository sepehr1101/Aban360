using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class WaterMeterReplacementsQueryService : WaterMeterReplacementsBase, IWaterMeterReplacementsQueryService
    {
        public WaterMeterReplacementsQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsDataOutputDto>> GetInfo(WaterMeterReplacementsInputDto input)
        {
            string query = GetDetailWithCteQuery(input.IsChangeDate);

            string reportTitle = input.IsChangeDate == true ? ReportLiterals.WaterMeterReplacements(ReportLiterals.ChangeDate) : ReportLiterals.WaterMeterReplacements(ReportLiterals.RegisterDate);

            IEnumerable<WaterMeterReplacementsDataOutputDto> waterMeterReplacementsData = await _sqlReportConnection.QueryAsync<WaterMeterReplacementsDataOutputDto>(query, input);
            WaterMeterReplacementsHeaderOutputDto waterMeterReplacementsHeader = new WaterMeterReplacementsHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = (waterMeterReplacementsData is not null && waterMeterReplacementsData.Any()) ? waterMeterReplacementsData.Count() : 0,
                RecordCount = (waterMeterReplacementsData is not null && waterMeterReplacementsData.Any()) ? waterMeterReplacementsData.Count() : 0,
                Title = reportTitle,

                SumCommercialUnit = waterMeterReplacementsData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = waterMeterReplacementsData.Sum(i => i.DomesticUnit),
                SumOtherUnit = waterMeterReplacementsData.Sum(i => i.OtherUnit),
                TotalUnit = waterMeterReplacementsData.Sum(i => i.TotalUnit)
            };

            var result = new ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsDataOutputDto>(
                reportTitle,
                waterMeterReplacementsHeader,
                waterMeterReplacementsData);

            return result;
        }
    }
}
