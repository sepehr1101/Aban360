using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Implementations
{
    internal sealed class ClientCommentQueryService : AbstractBaseConnection, IClientCommentQueryService
    {
        public ClientCommentQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<CustomerCommentGetDto>> Get(string billId)
        {
            string query = GetByBillIdQuery();
            IEnumerable<CustomerCommentGetDto> result = await _sqlReportConnection.QueryAsync<CustomerCommentGetDto>(query, new { billId });
            return result;
        }

        private string GetByBillIdQuery()
        {
            return @"Select 
                    	BillId,
                    	Comment,
                    	UserDisplayName,
                    	UserId,
                    	InsertDateTime
                    From [CustomerWarehouse].dbo.ClientComments
                    Where BillId=@billId
                    Order by InsertDateTime Desc";
        }
    }
}
