using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterModifiedBillsDetailQueryService : AbstractBaseConnection, IWaterModifiedBillsDetailQueryService
    {
        public WaterModifiedBillsDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsDetailDataOutputDto>> GetInfo(WaterModifiedBillsInputDto input)
        {
            string modifiedBills = GetWaterModifiedBillsQuery();
            IEnumerable<WaterModifiedBillsDetailDataOutputDto> modifiedBillsData = await _sqlReportConnection.QueryAsync<WaterModifiedBillsDetailDataOutputDto>(modifiedBills);//todo: Parameters
            WaterModifiedBillsHeaderOutputDto modifiedBillsHeader = new WaterModifiedBillsHeaderOutputDto()
            { };

            var result = new ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsDetailDataOutputDto>(ReportLiterals.WaterModifiedBillsDetail, modifiedBillsHeader, modifiedBillsData);
            return result;
        }

        private string GetWaterModifiedBillsQuery()
        {
            return @" ";
        }
    }
}
