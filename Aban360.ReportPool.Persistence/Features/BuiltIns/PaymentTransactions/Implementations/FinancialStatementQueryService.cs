using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class FinancialStatementQueryService : FinancialStatementBase, IFinancialStatementQueryService
    {
        public FinancialStatementQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<FinancialStatementDataOutputDto>> GetWaterTotal(FinancialStatementInputDto input)
        {
            string query = GetWaterTotalQuery();
            Console.WriteLine(query);
            IEnumerable<FinancialStatementDataOutputDto> result = await _sqlReportConnection.QueryAsync<FinancialStatementDataOutputDto>(query, input);
            return result;
        }
    }
}
