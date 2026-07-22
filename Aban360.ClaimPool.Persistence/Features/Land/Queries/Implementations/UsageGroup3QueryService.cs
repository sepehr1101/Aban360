using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class UsageGroup3QueryService : AbstractBaseConnection, IUsageGroup3QueryService
    {
        public UsageGroup3QueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<UsageGroup3GetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<UsageGroup3GetDto> result = await _sqlReportConnection.QueryAsync<UsageGroup3GetDto>(query);
            return result;
        }
        public async Task<UsageGroup3GetDto> Get(short id)
        {
            string query = GetByIdQuery();
            UsageGroup3GetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<UsageGroup3GetDto>(query, new { id });
            if (result is null)
            {
                throw new InvalidDataException(ExceptionLiterals.NotFoundId);
            }
            return result;
        }
        public async Task<UsageGroup3GetDto?> Get(UsageGroup3InsertDto input)
        {
            string query = GetDuplicateQuery();
            UsageGroup3GetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<UsageGroup3GetDto>(query, input);
            return result;
        }
        public async Task<IEnumerable<UsageGroup3GetDto>> GetByParrentIds(IEnumerable<short> ids)
        {
            string query = GetByParentIdsQuery();
            IEnumerable<UsageGroup3GetDto> result = await _sqlReportConnection.QueryAsync<UsageGroup3GetDto>(query, new { ids });
            return result;
        }
        public async Task<IEnumerable<UsageGroup3GetDto>> GetByParrentId(short id)
        {
            string query = GetByParentIdQuery();
            IEnumerable<UsageGroup3GetDto> result = await _sqlReportConnection.QueryAsync<UsageGroup3GetDto>(query, new { id });
            return result;
        }

        private string GetQuery()
        {
            return @"Select 
						 u1.Id Group1Id,
						 u1.Title Group1Title,
                    	 u3.Id,
						 u3.Group2Id,
						 u3.UsageId,
						 u3.UsageTitle,
						 u2.Title Group2Title
                    From [Db70].dbo.UsageGroup3 u3
					Join [Db70].dbo.UsageGroup2 u2
						ON u2.Id=u3.Group2Id
					Join [Db70].dbo.UsageGroup1 u1
						ON u1.Id=u2.Group1Id";
        }
        private string GetByIdQuery()
        {
            return @"Select 
						 u1.Id Group1Id, 
						 u1.Title Group1Title,
                    	 u3.Id,
						 u3.Group2Id,
						 u3.UsageId,
						 u3.UsageTitle,
						 u2.Title Group2Title
                    From [Db70].dbo.UsageGroup3 u3
					Join [Db70].dbo.UsageGroup2 u2
						ON u2.Id=u3.Group2Id
					Join [Db70].dbo.UsageGroup1 u1
						ON u1.Id=u2.Group1Id
                    Where u3.Id=@id";
        }
        private string GetDuplicateQuery()
        {
            return @"Select 
						 u1.Id Group1Id,
						 u1.Title Group1Title,
                    	 u3.Id,
						 u3.Group2Id,
						 u3.UsageId,
						 u3.UsageTitle,
						 u2.Title Group2Title
                    From [Db70].dbo.UsageGroup3 u3
					Join [Db70].dbo.UsageGroup2 u2
						ON u2.Id=u3.Group2Id
					Join [Db70].dbo.UsageGroup1 u1
						ON u1.Id=u2.Group1Id
                    Where 
                         u3.Group2Id = @Group2Id AND
						 u3.UsageId = @UsageId ";
        }
        private string GetByParentIdsQuery()
        {
                return @"Select 
						 u1.Id Group1Id,
						 u1.Title Group1Title,
                    	 u3.Id,
						 u3.Group2Id,
						 u3.UsageId,
						 u3.UsageTitle,
						 u2.Title Group2Title
                    From [Db70].dbo.UsageGroup3 u3
					Join [Db70].dbo.UsageGroup2 u2
						ON u2.Id=u3.Group2Id
					Join [Db70].dbo.UsageGroup1 u1
						ON u1.Id=u2.Group1Id
                    Where u3.Group2Id IN @ids";
        }
        private string GetByParentIdQuery()
        {
            return @"Select 
						 u1.Id Group1Id,
						 u1.Title Group1Title,
                    	 u3.Id,
						 u3.Group2Id,
						 u3.UsageId,
						 u3.UsageTitle,
						 u2.Title Group2Title
                    From [Db70].dbo.UsageGroup3 u3
					Join [Db70].dbo.UsageGroup2 u2
						ON u2.Id=u3.Group2Id
					Join [Db70].dbo.UsageGroup1 u1
						ON u1.Id=u2.Group1Id
                    Where u3.Group2Id = @id";
        }
    }
}
