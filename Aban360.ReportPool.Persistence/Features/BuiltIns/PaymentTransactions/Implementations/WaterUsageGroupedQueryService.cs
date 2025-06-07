using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class WaterUsageGroupedQueryService : AbstractBaseConnection, IWaterUsageGroupedQueryService
    {
        public WaterUsageGroupedQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterUsageGroupedHeaderOutputDto, WaterUsageGroupedDataOutputDto>> GetInfo(WaterUsageGroupedInputDto input)
        {
            string waterUsageGroupeds = GetWaterUsageGroupedQuery();
            IEnumerable<WaterUsageGroupedDataOutputDto> waterUsageGroupedDate = await _sqlConnection.QueryAsync<WaterUsageGroupedDataOutputDto>(waterUsageGroupeds);//todo: Parameters
            WaterUsageGroupedHeaderOutputDto waterUsageGroupedHeader = new WaterUsageGroupedHeaderOutputDto()
            { };

            var result = new ReportOutput<WaterUsageGroupedHeaderOutputDto, WaterUsageGroupedDataOutputDto>(ReportLiterals.WaterUsageGrouped, waterUsageGroupedHeader, waterUsageGroupedDate);
            return result;
        }

        private string GetWaterUsageGroupedQuery()
        {
            return @" ";
        }
    }
}
