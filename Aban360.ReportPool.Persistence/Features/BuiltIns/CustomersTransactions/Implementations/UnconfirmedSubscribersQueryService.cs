using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class UnconfirmedSubscribersQueryService : AbstractBaseConnection, IUnconfirmedSubscribersQueryService
    {
        public UnconfirmedSubscribersQueryService(IConfiguration configuration)
        : base(configuration)
        { }

        public async Task<ReportOutput<UnconfirmedSubscribersHeaderOutputDto, UnconfirmedSubscribersDataOutputDto>> GetInfo(UnconfirmedSubscribersInputDto input)
        {
            string unconfirmedSubscribersQuery = UnconfirmedSubscribersQuery();
            IEnumerable<UnconfirmedSubscribersDataOutputDto> unconfirmedSubscribersData = await _sqlConnection.QueryAsync<UnconfirmedSubscribersDataOutputDto>(unconfirmedSubscribersQuery);//todo: parameters
            UnconfirmedSubscribersHeaderOutputDto unconfirmedSubscribersHeader = new UnconfirmedSubscribersHeaderOutputDto()
            { };

            var result = new ReportOutput<UnconfirmedSubscribersHeaderOutputDto, UnconfirmedSubscribersDataOutputDto>
                (ReportLiterals.UnconfirmedSubscribers,
                 unconfirmedSubscribersHeader,
                 unconfirmedSubscribersData);

            return result;
        }

        private string UnconfirmedSubscribersQuery()
        {
            return " ";
        }
    }
}
