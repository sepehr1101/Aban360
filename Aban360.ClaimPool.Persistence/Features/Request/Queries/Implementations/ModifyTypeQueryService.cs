using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class ModifyTypeQueryService : AbstractBaseConnection, IModifyTypeQueryService
    {
        public ModifyTypeQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<ModifyTypeGetDto>> Get()
        {
            string query = GetQuery();
            return await _sqlReportConnection.QueryAsync<ModifyTypeGetDto>(query);
        }

        public async Task<ModifyTypeGetDto> GetByKarten75(int id)
        {
            string query = GetByIdQuery(true);
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<ModifyTypeGetDto>(query, new { id });
        }

        public async Task<ModifyTypeGetDto> GetByRequestBillDetails(int id)
        {
            string query = GetByIdQuery(false);
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<ModifyTypeGetDto>(query, new { id });
        }

        private string GetQuery()
        {
            return @"Select 
                    	RequestBillDetailsId,
                    	Karten75Id,
                    	Title
                    From [Db70].dbo.ModifyType";
        }
        private string GetByIdQuery(bool isKarten75Id)
        {
            string condition = isKarten75Id ? "Karten75Id" : "RequestBillDetailsId";
            return @$"Select 
                    	RequestBillDetailsId,
                    	Karten75Id,
                    	Title
                    From [Db70].dbo.ModifyType
                    Where {condition}=@id";
        }
    }
}
