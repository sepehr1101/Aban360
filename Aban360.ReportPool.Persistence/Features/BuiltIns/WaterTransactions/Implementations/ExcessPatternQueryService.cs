using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class ExcessPatternQueryService : AbstractBaseConnection, IExcessPatternQueryService
    {
        public ExcessPatternQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ExcessPatternHeaderOutputDto, ExcessPatternDataOutputDto>> GetInfo(ExcessPatternInputDto input)
        {
            string ExcessPatterns = GetExcessPatternQuery();
            IEnumerable<ExcessPatternDataOutputDto> unspecifiedWaterData = await _sqlReportConnection.QueryAsync<ExcessPatternDataOutputDto>(ExcessPatterns);//todo: Parameters
            ExcessPatternHeaderOutputDto unspecifiedWaterHeader = new ExcessPatternHeaderOutputDto()
            { };

            var result = new ReportOutput<ExcessPatternHeaderOutputDto, ExcessPatternDataOutputDto>(ReportLiterals.ExcessPattern, unspecifiedWaterHeader, unspecifiedWaterData);
            return result;
        }

        private string GetExcessPatternQuery()
        {
            return @" ";
        }
    }
}
