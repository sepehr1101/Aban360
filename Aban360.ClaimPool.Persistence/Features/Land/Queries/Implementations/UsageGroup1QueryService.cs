using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class UsageGroup1QueryService : AbstractBaseConnection, IUsageGroup1QueryService
    {
        public UsageGroup1QueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<UsageGroup1GetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<UsageGroup1GetDto> result = await _sqlReportConnection.QueryAsync<UsageGroup1GetDto>(query);
            return result;
        }

        public async Task<UsageGroup1GetDto> Get(int id)
        {
            string query = GetByIdQuery();
            UsageGroup1GetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<UsageGroup1GetDto>(query, new { id });
            if (result is null)
            {
                throw new InvalidDataException(ExceptionLiterals.NotFoundId);
            }
            return result;
        }
        public async Task<UsageGroup1GetDto?> Get(string title)
        {
            string query = GetByTitleQuery();
            UsageGroup1GetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<UsageGroup1GetDto>(query, new { title });
            return result;
        }

        private string GetQuery()
        {
            return @"Select 
                    	Id,
                    	Title
                    From [Db70].dbo.UsageGroup1";
        }
        private string GetByIdQuery()
        {
            return @"Select 
                    	 Id,
                    	 Title
                    From [Db70].dbo.UsageGroup1
                    Where Id=@id";
        }
        private string GetByTitleQuery()
        {
            return @"Select 
                    	 Id,
                    	 Title
                    From [Db70].dbo.UsageGroup1
                    Where Title=@Title";
        }
    }
}
