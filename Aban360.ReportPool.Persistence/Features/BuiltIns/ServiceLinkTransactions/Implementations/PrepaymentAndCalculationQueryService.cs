using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class PrepaymentAndCalculationQueryService : AbstractBaseConnection, IPrepaymentAndCalculationQueryService
    {
        public PrepaymentAndCalculationQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto>> GetInfo(PrepaymentAndCalculationInputDto input)
        {
            string prepaymentAndCalculationQueryString = GetPrepaymentAndCalculationDataQuery();
            IEnumerable<PrepaymentAndCalculationDataOutputDto> prepaymentAndCalculationData = await _sqlConnection.QueryAsync<PrepaymentAndCalculationDataOutputDto>(prepaymentAndCalculationQueryString);//todo: parameters
            PrepaymentAndCalculationHeaderOutputDto prepaymentAndCalculationHeader = new PrepaymentAndCalculationHeaderOutputDto()
            { };

            var result = new ReportOutput<PrepaymentAndCalculationHeaderOutputDto, PrepaymentAndCalculationDataOutputDto>(ReportLiterals.PrepaymentAndCalculation, prepaymentAndCalculationHeader, prepaymentAndCalculationData);

            return result;
        }

        private string GetPrepaymentAndCalculationDataQuery()
        {
            return " ";
        }

    }
}
