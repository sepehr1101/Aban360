using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Implementations
{
    internal sealed class BillReturnCauseQueryService : AbstractBaseConnection, IBillReturnCauseQueryService
    {
        public BillReturnCauseQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<BillReturnCauseGetDto> Get(SearchShortInputDto input)
        {
            string query = GetSingleQuery();
            BillReturnCauseGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<BillReturnCauseGetDto>(query, input);

            return result;
        }

        public async Task<IEnumerable<BillReturnCauseGetDto>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<BillReturnCauseGetDto> result = await _sqlReportConnection.QueryAsync<BillReturnCauseGetDto>(query, null);

            return result;
        }

        private string GetSingleQuery()
        {
            return @"Select *
                    From [Db70].dbo.BillReturnCause
                    Where 
                        RemoveDateTime IS NULL AND    
                        Id=@Id";
        }
        private string GetAllQuery()
        {
            return @"Select *
                    From [Db70].dbo.BillReturnCause
                    Where RemoveDateTime IS NULL";
        }
    }
}
