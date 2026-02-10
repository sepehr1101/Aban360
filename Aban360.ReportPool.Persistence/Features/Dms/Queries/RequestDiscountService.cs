using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.Dms;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Dms.Queries
{
    public interface IRequestDiscountService
    {
        Task<IEnumerable<ClientDiscount>> Get();
    }

    internal sealed class RequestDiscountService : AbstractBaseConnection, IRequestDiscountService
    {
        public RequestDiscountService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<ClientDiscount>> Get()
        {
            IEnumerable<ClientDiscount> data = await _sqlReportConnection.QueryAsync<ClientDiscount>(GetQuery());
            return data;
        }
        private string GetQuery()
        {
            string query = "SELECT * FROM AbAndFazelab.[dbo].[ClientDiscount]";
            return query;
        }
    }
}
