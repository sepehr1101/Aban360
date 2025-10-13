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
    internal sealed class SewageWaterDistanceofRequestAndInstallationDetailQueryService : SewageWaterDistanceofRequestAndInstallationBase, ISewageWaterDistanceofRequestAndInstallationDetailQueryService
    {
        public SewageWaterDistanceofRequestAndInstallationDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto>> Get(SewageWaterDistanceofRequestAndInstallationInputDto input)
        {
            string query = GetDetailsQuery(input.IsWater, input.IsInstallation);
            string reportTitle = GetTitle(input.IsWater, input.IsInstallation);

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
                fromReadingNumber=input.FromReadingNumber,
                toReadingNumber=input.ToReadingNumber,
                usageIds = input.UsageIds
            };
            IEnumerable<SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto>(query, @params);
            SewageWaterDistanceofRequestAndInstallationHeaderOutputDto RequestHeader = new SewageWaterDistanceofRequestAndInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber=input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0,
                CustomerCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0,
                Title= reportTitle, 

                SumCommercialUnit = RequestData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = RequestData.Sum(i => i.DomesticUnit),
                SumOtherUnit = RequestData.Sum(i => i.OtherUnit),
                TotalUnit = RequestData.Sum(i => i.TotalUnit)
            };


            var result = new ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationDetailDataOutputDto>(reportTitle, RequestHeader, RequestData);

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
