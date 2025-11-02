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

        public async Task<Article11OutputDto> Get(short id)
        {
            string query = GetQueryById();
            Article11OutputDto article11 = await _sqlConnection.QueryFirstOrDefaultAsync<Article11OutputDto>(query, new { id = id });
            if (article11 == null)
            {
                throw new InvalidIdException();
            }
            return article11;
        }
        public async Task<IEnumerable<Article11OutputDto>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<Article11OutputDto> article11 = await _sqlConnection.QueryAsync<Article11OutputDto>(query, null);

            return article11;
        }

        private string GetQuery()
        {
            return @"Select *
                    From [Aban360].CalculationPool.Article11 
                    Where
                    	[CustomerWarehouse].dbo.PersianToMiladi(ToDateJalali)>GETDATE() AND
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
                    	[CustomerWarehouse].dbo.PersianToMiladi(ToDateJalali)>GETDATE() AND
                    	RemoveDateTime IS NULL AND
						Id=@id";
        }

        private string GetAllQuery()
        {
            return @"Select *
                    From [Aban360].CalculationPool.Article11 
                    Where
                    	[CustomerWarehouse].dbo.PersianToMiladi(ToDateJalali)>GETDATE() AND
                    	RemoveDateTime IS NULL";
        }
    }
}
