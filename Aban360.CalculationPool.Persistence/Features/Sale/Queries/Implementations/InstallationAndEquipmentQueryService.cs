using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Exceptions;
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
        public async Task<InstallationAndEquipmentOutputDto> Get(int id, string currentDateJalali)
        {
            string query = GetQueryById();
            InstallationAndEquipmentOutputDto data = await _sqlConnection.QueryFirstAsync<InstallationAndEquipmentOutputDto>(query, new { id = id, CurrentDateJalali= currentDateJalali });
            if (data == null)
            {
                throw new InvalidIdException();
            }
            return data;
        }
        public async Task<IEnumerable<InstallationAndEquipmentOutputDto>> Get(string currentDateJalali)
        {
            string query = GetAllQuery();
            IEnumerable<InstallationAndEquipmentOutputDto> data = await _sqlConnection.QueryAsync<InstallationAndEquipmentOutputDto>(query, new { CurrentDateJalali=currentDateJalali });

            return data;
        }

        private string GetSaleQuery()
        {
            return @"Select *
                    From [Aban360].CalculationPool.InstallationAndEquipment i
                    Where 
                    	i.RemoveDateTime IS NULL AND
                    	i.ToDateJalali>@currentDateJalali AND
                    	i.IsWater=@isWater AND
                    	i.DiameterId=@diameterId ";
        }

        private string GetQueryById()
        {
            return @"Select *
                    From [Aban360].CalculationPool.InstallationAndEquipment i
                    Where 
                    	i.RemoveDateTime IS NULL AND
                    	i.ToDateJalali>@CurrentDateJalali AND
                    	i.Id=@id";
        }

        private string GetAllQuery()
        {
            return @"Select *
                    From [Aban360].CalculationPool.InstallationAndEquipment i
                    Where 
                    	i.RemoveDateTime IS NULL AND
                    	i.ToDateJalali>@CurrentDateJalali";
        }

    }
}
