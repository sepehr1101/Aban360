using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class UnpaidQueryService : AbstractBaseConnection, IUnpaidQueryService
    {
        public UnpaidQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<UnpaidHeaderOutputDto, UnpaidDataOutputDto>> GetInfo(UnpaidInputDto input)
        {
            string unpaids = GetUnpaidQuery();
            IEnumerable<UnpaidDataOutputDto> unpaidData = await _sqlConnection.QueryAsync<UnpaidDataOutputDto>(unpaids);//todo: Parameters
            UnpaidHeaderOutputDto unpaidHeader = new UnpaidHeaderOutputDto()
            { };

            var result = new ReportOutput<UnpaidHeaderOutputDto, UnpaidDataOutputDto>(ReportLiterals.Unpaid, unpaidHeader, unpaidData);
            return result;
        }

        private string GetUnpaidQuery()
        {
            return @" ";
        }
    }
}
