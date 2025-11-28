using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Commands.Implementations
{
    internal sealed class TankerWaterDistanceTariffCommandService : AbstractBaseConnection, ITankerWaterDistanceTariffCommandService
    {
        public TankerWaterDistanceTariffCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Create(TankerWaterDistanceTariffCreateDto input)
        {
            string query = InsertQuery();
            await _sqlConnection.ExecuteAsync(query, input);
        }
        public async Task Update(TankerWaterDistanceTariffInputDto input)
        {
            string query = UpdateQuery();
            await _sqlConnection.ExecuteScalarAsync(query, input);
        }
        public async Task Delete(DeleteDto input)
        {
            string query = DeleteQuery();
            await _sqlConnection.ExecuteScalarAsync(query, input);
        }

        private string InsertQuery()
        {
            return @"Insert Into Aban360.CalculationPool.TankerWaterDistanceTariff(
                        FromDistance,ToDistance,Amount,FromDateJalali,ToDateJalali,
                        RegisterByUserId,RegisterDateTime,RemoveByUserId,RemoveDateTime)
                    Values(
                        @FromDistance,@ToDistance,@Amount,@FromDateJalali,@ToDateJalali,
                        @RegisterByUserId,@RegisterDateTime,@RemoveByUserId,@RemoveDateTime)";
        }
        private string DeleteQuery()
        {
            return @"Update Aban360.CalculationPool.TankerWaterDistanceTariff
                    Set 
                    	RemoveByUserId=@RemoveByUserId ,
                    	RemoveDateTime=@RemoveDateTime
                    Where Id=@Id";
        }
        private string UpdateQuery()
        {
            return @"Update Aban360.CalculationPool.TankerWaterDistanceTariff
                    Set 
                    	FromDistance=@FromDistance ,
                    	ToDistance=@ToDistance,
                    	Amount=@Amount,
                    	FromDateJalali=@FromDateJalali,
                    	ToDateJalali=@ToDateJalali
                    Where Id=@Id";
        }

    }
}
