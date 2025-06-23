using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class HouseholdNumberQueryService : AbstractBaseConnection, IHouseholdNumberQueryService
    {
        public HouseholdNumberQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberDataOutputDto>> GetInfo(HouseholdNumberInputDto input)
        {
            string householdNumberQuery = GetHouseholdNumberQuery();
            IEnumerable<HouseholdNumberDataOutputDto> householdNumberData = await _sqlReportConnection.QueryAsync<HouseholdNumberDataOutputDto>(householdNumberQuery);//todo: params
            HouseholdNumberHeaderOutputDto householdNumberHeader = new HouseholdNumberHeaderOutputDto()
            { };

            var result = new ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberDataOutputDto>(ReportLiterals.HouseholdNumber, householdNumberHeader, householdNumberData);

            return result;
        }

        private string GetHouseholdNumberQuery()
        {
            return @"";
        }
    }
}
