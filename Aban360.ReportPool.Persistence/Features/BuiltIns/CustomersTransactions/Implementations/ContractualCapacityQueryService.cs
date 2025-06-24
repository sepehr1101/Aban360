using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class ContractualCapacityQueryService : AbstractBaseConnection, IContractualCapacityQueryService
    {
        public ContractualCapacityQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto>> GetInfo(ContractualCapacityInputDto input)
        {
            string contractualCapacityQuery = GetContractualCapacityQuery();
            IEnumerable<ContractualCapacityDataOutputDto> contractualCapacityData = await _sqlReportConnection.QueryAsync<ContractualCapacityDataOutputDto>(contractualCapacityQuery);//todo: params
            ContractualCapacityHeaderOutputDto contractualCapacityHeader = new ContractualCapacityHeaderOutputDto()
            { };

            var result = new ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto>(ReportLiterals.ContractualCapacity, contractualCapacityHeader, contractualCapacityData);

            return result;
        }

        private string GetContractualCapacityQuery()
        {
            return @"";
        }
    }
}
