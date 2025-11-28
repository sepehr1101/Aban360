using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Operation.Distance;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Implementations
{
    internal sealed class TankerWaterDistanceTariffQueryService : AbstractBaseConnection, ITankerWaterDistanceTariffQueryService
    {
        public TankerWaterDistanceTariffQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<TankerWaterDistanceTariffOutputDto> Get(short id)
        {
            string query = GetQuery();
            TankerWaterDistanceTariffOutputDto TankerWaterDistanceTariff = await _sqlConnection.QueryFirstOrDefaultAsync<TankerWaterDistanceTariffOutputDto>(query, new { id });

            return TankerWaterDistanceTariff;
        }
        public async Task<IEnumerable<TankerWaterDistanceTariffOutputDto>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<TankerWaterDistanceTariffOutputDto> TankerWaterDistanceTariff = await _sqlConnection.QueryAsync<TankerWaterDistanceTariffOutputDto>(query, null);

            return TankerWaterDistanceTariff;
        }
        public async Task<TankerWaterDistanceTariffOutputDto> Get(int distance, string currentDateJalali)
        {
            string query = GetByDistanceQuery();
            var @params = new
            {
                distance,
                currentDateJalali,
            };
            TankerWaterDistanceTariffOutputDto result = await _sqlConnection.QueryFirstOrDefaultAsync<TankerWaterDistanceTariffOutputDto>(query, @params);
            return result;
        }

        private string GetQuery()
        {
            return @"Select *
                    From Aban360.CalculationPool.TankerWaterDistanceTariff
                    Where
                    	Id=@id AND
                    	RemoveByUserId IS NULL AND
                    	RemoveDateTime IS NULL";
        }
        private string GetAllQuery()
        {
            return @"Select *
                    From Aban360.CalculationPool.TankerWaterDistanceTariff
                    Where
                    	RemoveByUserId IS NULL AND
                    	RemoveDateTime IS NULL";
        }
        private string GetByDistanceQuery()
        {
            return @"Select *
                    From Aban360.CalculationPool.TankerWaterDistanceTariff
                    Where 
                    	@distance>FromDistance AND @distance<=ToDistance AND
	                    ToDateJalali>=@currentDateJalali AND
                    	RemoveByUserId IS NULL AND RemoveDateTime IS NULL";
        }
    }
}
