using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class DebtorByDayQueryService : AbstractBaseConnection, IDebtorByDayQueryService
    {
        public DebtorByDayQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDayDataOutputDto>> GetInfo(DebtorByDayInputDto input)
        {
            string debtorByDayQueryString = GetDebtorByDayDataQuery();
            IEnumerable<DebtorByDayDataOutputDto> debtorByDayData = await _sqlConnection.QueryAsync<DebtorByDayDataOutputDto>(debtorByDayQueryString);//todo: parameters
            DebtorByDayHeaderOutputDto debtorByDayHeader = new DebtorByDayHeaderOutputDto()
            { };

            var result = new ReportOutput<DebtorByDayHeaderOutputDto, DebtorByDayDataOutputDto>(ReportLiterals.DebtorByDay, debtorByDayHeader, debtorByDayData);

            return result;
        }

        private string GetDebtorByDayDataQuery()
        {
            return " ";
        }

    }
}
