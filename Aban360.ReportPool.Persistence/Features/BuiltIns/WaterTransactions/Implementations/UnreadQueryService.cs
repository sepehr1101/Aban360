using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class UnreadQueryService : AbstractBaseConnection, IUnreadQueryService
    {
        public UnreadQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto>> GetInfo(UnreadInputDto input)
        {
            string unread = GetUnreadQuery();
            IEnumerable<UnreadDataOutputDto> unreadData = await _sqlConnection.QueryAsync<UnreadDataOutputDto>(unread);//todo: Parameters
            UnreadHeaderOutputDto unreadHeader = new UnreadHeaderOutputDto()
            { };

            var result = new ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto>(ReportLiterals.Unread, unreadHeader, unreadData);
            return result;
        }

        private string GetUnreadQuery()
        {
            return @" ";
        }
    }
}
