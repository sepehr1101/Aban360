using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class ModifiedBillsQueryService : AbstractBaseConnection, IModifiedBillsQueryService
    {
        public ModifiedBillsQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ModifiedBillsHeaderOutputDto, ModifiedBillsDataOutputDto>> GetInfo(ModifiedBillsInputDto input)
        {
            string modifiedBills = GetModifiedBillsQuery();
            IEnumerable<ModifiedBillsDataOutputDto> modifiedBillsData = await _sqlConnection.QueryAsync<ModifiedBillsDataOutputDto>(modifiedBills);//todo: Parameters
            ModifiedBillsHeaderOutputDto modifiedBillsHeader = new ModifiedBillsHeaderOutputDto()
            { };

            var result = new ReportOutput<ModifiedBillsHeaderOutputDto, ModifiedBillsDataOutputDto>(ReportLiterals.ModifiedBills, modifiedBillsHeader, modifiedBillsData);//Todo:Switch Title
            return result;
        }

        private string GetModifiedBillsQuery()
        {
            return @" ";
        }
    }
}
