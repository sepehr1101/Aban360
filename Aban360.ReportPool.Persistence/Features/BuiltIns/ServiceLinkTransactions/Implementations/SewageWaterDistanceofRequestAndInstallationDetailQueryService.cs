using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
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
    internal sealed class SewageWaterDistanceofRequestAndInstallationDetailQueryService : SewageWaterDistanceofRequestAndInstallationBase, ISewageWaterDistanceofRequestAndInstallationDetailQueryService
    {
        public SewageWaterDistanceofRequestAndInstallationDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceDetailDataOutputDto>> Get(SewageWaterDistanceofRequestAndInstallationInputDto input)
        {
            string query = GetDetailsQuery(input.IsWater, input.IsInstallation);
            string reportTitle = GetTitle(input.IsWater, input.IsInstallation);

            IEnumerable<SewageWaterDistanceDetailDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterDistanceDetailDataOutputDto>(query, input);
            SewageWaterDistanceHeaderOutputDto RequestHeader = new SewageWaterDistanceHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber=input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = RequestData.CountValue(),
                CustomerCount = RequestData.CountValue(),
                Title= reportTitle, 

                SumCommercialUnit = RequestData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = RequestData.Sum(i => i.DomesticUnit),
                SumOtherUnit = RequestData.Sum(i => i.OtherUnit),
                TotalUnit = RequestData.Sum(i => i.TotalUnit)
            };
            var result = new ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceDetailDataOutputDto>(reportTitle, RequestHeader, RequestData);
            return result;
        }
       
        private string GetTitle(bool IsWater, bool IsInstallation)
        {
            if (IsWater)
            {
                return IsInstallation ? ReportLiterals.WaterDistanceInstallationRegisterDetail : ReportLiterals.WaterDistanceRequestRegisterDetail;
            }
            else
            {
                return IsInstallation ? ReportLiterals.SewageDistanceInstallationeRegisterDetail : ReportLiterals.SewageDistanceRequesteRegisterDetail;
            }
        }
    }
}
