using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Db.Dapper;
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
                throw new InvalidDataException();//todo:personalized exception
            }
            return article11;
        }

        private string GetQuery()
        {
            return @"Select *
                    From [Aban360].CalculationPool.Article11 
                    Where
                    	[CustomerWarehouse].dbo.PersianToMiladi(ToDateJalali)>GETDATE() AND
                    	RemovedDateJalali IS NULL AND
                    	IsDomestic=@isDomestic AND
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
                    	RemovedDateJalali IS NULL AND
						Id=@id";
        }
            
    }
}
