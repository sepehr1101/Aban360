using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Implementations
{
    internal sealed class SaleQueryService : AbstractBaseConnection, ISaleQueryService
    {
        public SaleQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Get(SaleInputDto input)
        {
            string query = GetInstalltionAndEquipmentQuery();
            var installationAndEquipment = await _sqlConnection.QueryAsync<int>(query, new { });
        }

        private string GetInstalltionAndEquipmentQuery()
        {
            return @"Select *
                    From [Aban360].CalculationPool.InstallationAndEquipment i
                    Where 
                    	i.RemovedDateJalali IS NULL AND
                    	[CustomerWarehouse].dbo.PersianToMiladi(i.ToDateJalali)>GETDATE() AND
                    	i.IsWater=1 AND
                    	i.MeterDiameterId=1";
        }
    }
}
