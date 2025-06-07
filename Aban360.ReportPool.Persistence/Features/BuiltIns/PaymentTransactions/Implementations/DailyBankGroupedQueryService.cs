using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class DailyBankGroupedQueryService : AbstractBaseConnection, IDailyBankGroupedQueryService
    {
        public DailyBankGroupedQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<DailyBankGroupedHeaderOutputDto, DailyBankGroupedDataOutputDto>> GetInfo(DailyBankGroupedInputDto input)
        {
            string dailyBankGroupeds = GetDailyBankGroupedQuery();
            IEnumerable<DailyBankGroupedDataOutputDto> dailyBankGroupedDate = await _sqlConnection.QueryAsync<DailyBankGroupedDataOutputDto>(dailyBankGroupeds);//todo: Parameters
            DailyBankGroupedHeaderOutputDto dailyBankGroupedHeader = new DailyBankGroupedHeaderOutputDto()
            { };

            var result = new ReportOutput<DailyBankGroupedHeaderOutputDto, DailyBankGroupedDataOutputDto>(ReportLiterals.DailyBankGrouped, dailyBankGroupedHeader, dailyBankGroupedDate);
            return result;
        }

        private string GetDailyBankGroupedQuery()
        {
            return @" ";
        }
    }
}
