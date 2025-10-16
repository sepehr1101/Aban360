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
    internal sealed class SewageWaterRequestNonInstalledDetailQueryService : NonInstalledBase, ISewageWaterRequestNonInstalledDetailQueryService
    {
        public SewageWaterRequestNonInstalledDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledDetailDataOutputDto>> Get(SewageWaterRequestNonInstalledInputDto input)
        {
            string query = GetDetailsQuery(input.IsWater);
            string reportTitle = input.IsWater ? ReportLiterals.WaterRequestNonInstalledDetail : ReportLiterals.SewageRequestNonInstalledDetail;

            IEnumerable<SewageWaterRequestNonInstalledDetailDataOutputDto> requestNonInstalledData = await _sqlReportConnection.QueryAsync<SewageWaterRequestNonInstalledDetailDataOutputDto>(query, input);
            SewageWaterRequestNonInstalledHeaderOutputDto requestNonInstalledHeader = new SewageWaterRequestNonInstalledHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (requestNonInstalledData is not null && requestNonInstalledData.Any()) ? requestNonInstalledData.Count() : 0,
                Title = reportTitle,

                SumCommercialUnit = requestNonInstalledData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = requestNonInstalledData.Sum(i => i.DomesticUnit),
                SumOtherUnit = requestNonInstalledData.Sum(i => i.OtherUnit),
                TotalUnit = requestNonInstalledData.Sum(i => i.TotalUnit),
                CustomerCount = (requestNonInstalledData is not null && requestNonInstalledData.Any()) ? requestNonInstalledData.Count() : 0,
            };
            var result = new ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledDetailDataOutputDto>
                (reportTitle,
                requestNonInstalledHeader,
                requestNonInstalledData);

            return result;
        }
    }
}
