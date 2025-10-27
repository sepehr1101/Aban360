using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Implementations
{
    internal sealed class InstallationAndEquipmentQueryService : AbstractBaseConnection, IInstallationAndEquipmentQueryService
    {
        public InstallationAndEquipmentQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<InstallationAndEquipmentOutputDto> Get(InstallationAndEquipmentGetDto input)
        {
            string query = GetSaleQuery();
            InstallationAndEquipmentOutputDto data = await _sqlConnection.QueryFirstAsync<InstallationAndEquipmentOutputDto>(query, input);

            return data;
        }
        public async Task<InstallationAndEquipmentOutputDto> Get(int id)
        {
            string query = GetQueryById();
            InstallationAndEquipmentOutputDto data = await _sqlConnection.QueryFirstAsync<InstallationAndEquipmentOutputDto>(query, new { id = id });
            if (data == null)
            {
                throw new InvalidDataException();
            }
            return data;
        }

        private string GetSaleQuery()
        {
            return @"Select *
                    From [Aban360].CalculationPool.InstallationAndEquipment i
                    Where 
                    	i.RemovedDateJalali IS NULL AND
                    	[CustomerWarehouse].dbo.PersianToMiladi(i.ToDateJalali)>GETDATE() AND
                    	i.IsWater=@isWater AND
                    	i.MeterDiameterId=@meterDiameterId ";
        }

        private string GetQueryById()
        {
            return @"Select *
                    From [Aban360].CalculationPool.InstallationAndEquipment i
                    Where 
                    	i.RemovedDateJalali IS NULL AND
                    	[CustomerWarehouse].dbo.PersianToMiladi(i.ToDateJalali)>GETDATE() AND
                    	i.Id=@id";
        }

    }
}
