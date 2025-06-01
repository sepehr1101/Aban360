using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class CalculationDetailsQueryService : AbstractBaseConnection, ICalculationDetailsQueryService
    {
        public CalculationDetailsQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<CalculationDetailsHeaderOutputDto, CalculationDetailsDataOutputDto>> GetInfo(CalculationDetailsInputDto input)
        {
            string calculationDetailsDataInfoQuery = GetCalculationDetailsDataQuery();
            IEnumerable<CalculationDetailsDataOutputDto> calculationDetailsData = await _sqlConnection.QueryAsync<CalculationDetailsDataOutputDto>(calculationDetailsDataInfoQuery);//todo: send parameters
            CalculationDetailsHeaderOutputDto calculationDetailsHeader = new CalculationDetailsHeaderOutputDto()
            { };

            var result = new ReportOutput<CalculationDetailsHeaderOutputDto, CalculationDetailsDataOutputDto>(ReportLiterals.CalculationDetails, calculationDetailsHeader, calculationDetailsData);
            return result;
        }

        private string GetCalculationDetailsDataQuery()
        {
            return " ";
        }
    }
}
