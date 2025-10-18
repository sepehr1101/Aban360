using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class SewageWaterRequestDetailQueryService : RequestOrInstallBase, ISewageWaterRequestDetailQueryService
    {
        public SewageWaterRequestDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestDetailDataOutputDto>> Get(SewageWaterRequestInputDto input)
        {
            string query = GetDetailsQuery(input.IsWater, InstallOrRequestOrInstallDepartmentEnum.Request);
            string reportTitle = input.IsWater ? ReportLiterals.WaterRequestDetail : ReportLiterals.SewageRequestDetail;

            IEnumerable<SewageWaterRequestDetailDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterRequestDetailDataOutputDto>(query, input);
            SewageWaterRequestHeaderOutputDto RequestHeader = new SewageWaterRequestHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0,
                Title = reportTitle,

                SumCommercialUnit = RequestData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = RequestData.Sum(i => i.DomesticUnit),
                SumOtherUnit = RequestData.Sum(i => i.OtherUnit),
                TotalUnit = RequestData.Sum(i => i.TotalUnit),
                CustomerCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0,
            };
            var result = new ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestDetailDataOutputDto>
                (reportTitle,
                RequestHeader,
                RequestData);

            return result;
        }
    }
}
