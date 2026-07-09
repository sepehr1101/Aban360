using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class UsageGroup2QueryService : AbstractBaseConnection, IUsageGroup2QueryService
    {
        public UsageGroup2QueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<UsageGroup2GetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<UsageGroup2GetDto> result = await _sqlReportConnection.QueryAsync<UsageGroup2GetDto>(query);
            return result;
        }
        public async Task<UsageGroup2GetDto> Get(short id)
        {
            string query = GetByIdQuery();
            UsageGroup2GetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<UsageGroup2GetDto>(query, new { id });
            if (result is null)
            {
                throw new InvalidDataException(ExceptionLiterals.NotFoundId);
            }
            return result;
        }
        public async Task<UsageGroup2GetDto?> Get(string title, short group1Id)
        {
            string query = GetByTitleAndGroup1IdQuery();
            UsageGroup2GetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<UsageGroup2GetDto>(query, new { title, group1Id });
            return result;
        }
        public async Task<IEnumerable<UsageGroup2GetDto>> GetByParentId(short group1Id)
        {
            string query = GetByGroup1IdQuery();
            IEnumerable<UsageGroup2GetDto> result = await _sqlReportConnection.QueryAsync<UsageGroup2GetDto>(query, new { group1Id});
            return result;
        }
        private string GetQuery()
        {
            return @"Select 
                    	u1.Id,
                    	u1.Title,
						u1.Group1Id,
						u2.Title Group1Title
                    From [Db70].dbo.UsageGroup2 u1
					Join [Db70].dbo.UsageGroup1 u2
						ON u2.Id=u1.Group1Id";
        }
        private string GetByIdQuery()
        {
            return @"Select 
                    	 u1.Id,
                    	 u1.Title,
						 u1.Group1Id,
						 u2.Title Group1Title
                    From [Db70].dbo.UsageGroup2 u1
					Join [Db70].dbo.UsageGroup1 u2
						ON u2.Id=u1.Group1Id
                    Where u1.Id = @id";
        }
        private string GetByTitleAndGroup1IdQuery()
        {
            return @"Select 
                    	 u1.Id,
                    	 u1.Title,
						 u1.Group1Id,
						 u2.Title Group1Title
                    From [Db70].dbo.UsageGroup2 u1
					Join [Db70].dbo.UsageGroup1 u2
						ON u2.Id=u1.Group1Id
                    Where 
                         u1.Title = @Title AND
						 u1.Group1Id = @Group1Id";
        }
        private string GetByGroup1IdQuery()
        {
            return @"Select 
                    	 u1.Id,
                    	 u1.Title,
						 u1.Group1Id,
						 u2.Title Group1Title
                    From [Db70].dbo.UsageGroup2 u1
					Join [Db70].dbo.UsageGroup1 u2
						ON u2.Id=u1.Group1Id
                    Where 
						 u1.Group1Id = @Group1Id";
        }
    }
}
