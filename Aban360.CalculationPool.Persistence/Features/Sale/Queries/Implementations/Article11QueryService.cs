using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Implementations
{
    internal sealed class Article11QueryService : AbstractBaseConnection, IArticle11QueryService
    {
        public Article11QueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<Article11OutputDto> Get(Article11GetDto input)
        {
            string query = GetQuery();
            Article11OutputDto article11 = await _sqlConnection.QueryFirstOrDefaultAsync<Article11OutputDto>(query, input);

            return article11;
        }

        public async Task<Article11OutputDto> Get(short id, string currentDateJalali)
        {
            string query = GetQueryById();
            Article11OutputDto article11 = await _sqlConnection.QueryFirstOrDefaultAsync<Article11OutputDto>(query, new { id = id, CurrentDateJalali = currentDateJalali });
            if (article11 == null)
            {
                throw new InvalidIdException();
            }
            return article11;
        }
        public async Task<IEnumerable<Article11OutputDto>> Get(string currentDateJalali)
        {
            string query = GetAllQuery();
            IEnumerable<Article11OutputDto> article11 = await _sqlConnection.QueryAsync<Article11OutputDto>(query, new { CurrentDateJalali = currentDateJalali });

            return article11;
        }
        public async Task<bool> ZoneWithBlockValidation(int zoneId, string? block)
        {
            string blockCondition = block is null ? "BlockCode IS NULL" : "BlockCode=@BlockCode";
            string query = GetZoneWithBlockValidationQuery(blockCondition);

            int count = await _sqlConnection.QueryFirstOrDefaultAsync<int>(query, new { zoneId = zoneId, BlockCode = block });
            return count <= 0 ? false : true;
        }
        private string GetQuery()
        {
            return @"Select *
                    From [Aban360].CalculationPool.Article11 
                    Where
                    	ToDateJalali>@CurrentDateJalali AND
                    	RemoveDateTime IS NULL AND
                    	(
                    		@BlockCode IS NULL OR
                    		BlockCode=@BlockCode
                    	)AND
                    	ZoneId=@zoneId";
        }
        private string GetQueryById()
        {
            return @"Select *
                    From [Aban360].CalculationPool.Article11 
                    Where
                    	ToDateJalali>@CurrentDateJalali AND
                    	RemoveDateTime IS NULL AND
						Id=@id";
        }
        private string GetAllQuery()
        {
            return @"Select *
                    From [Aban360].CalculationPool.Article11 
                    Where
                    	ToDateJalali>@CurrentDateJalali AND
                    	RemoveDateTime IS NULL";
        }
        private string GetZoneWithBlockValidationQuery(string blockCondition)
        {
            return @$"Select COUNT(1)
                    From Aban360.CalculationPool.Article11
                    Where 
                    	ZoneId=@ZoneId AND
                    	{blockCondition}";
        }
    }
}
