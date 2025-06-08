using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class WaterMeterReplacementsQueryService : AbstractBaseConnection, IWaterMeterReplacementsQueryService
    {
        public WaterMeterReplacementsQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsDataOutputDto>> GetInfo(WaterMeterReplacementsInputDto input)
        {
            string waterMeterReplacementss = GetWaterMeterReplacementsQuery();
            IEnumerable<WaterMeterReplacementsDataOutputDto> waterMeterReplacementsData = await _sqlConnection.QueryAsync<WaterMeterReplacementsDataOutputDto>(waterMeterReplacementss);//todo: Parameters
            WaterMeterReplacementsHeaderOutputDto waterMeterReplacementsHeader = new WaterMeterReplacementsHeaderOutputDto()
            { };

            var result = new ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsDataOutputDto>(ReportLiterals.WaterMeterReplacements, waterMeterReplacementsHeader, waterMeterReplacementsData);
            return result;
        }

        private string GetWaterMeterReplacementsQuery()
        {
            return @" ";
        }
    }
}
