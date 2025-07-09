using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterModifiedBillsSummaryQueryService : AbstractBaseConnection, IWaterModifiedBillsSummaryQueryService
    {
        public WaterModifiedBillsSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsSummaryDataOutputDto>> GetInfo(WaterModifiedBillsInputDto input)
        {
            string modifiedBills = GetWaterModifiedBillsQuery();
            IEnumerable<WaterModifiedBillsSummaryDataOutputDto> modifiedBillsData = await _sqlReportConnection.QueryAsync<WaterModifiedBillsSummaryDataOutputDto>(modifiedBills);//todo: Parameters
            WaterModifiedBillsHeaderOutputDto modifiedBillsHeader = new WaterModifiedBillsHeaderOutputDto()
            { };

            var result = new ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsSummaryDataOutputDto>(ReportLiterals.WaterModifiedBillsSummary, modifiedBillsHeader, modifiedBillsData);
            return result;
        }

        private string GetWaterModifiedBillsQuery()
        {
            return @" ";
        }
    }
}
