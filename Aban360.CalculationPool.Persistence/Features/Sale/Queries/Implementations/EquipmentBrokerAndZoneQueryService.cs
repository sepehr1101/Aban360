using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Implementations
{
    internal sealed class EquipmentBrokerAndZoneQueryService : AbstractBaseConnection, IEquipmentBrokerAndZoneQueryService
    {
        public EquipmentBrokerAndZoneQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<EquipmentBrokerOutputDto> Get(int zoneId)
        {
            string query = GetQuery();
            EquipmentBrokerOutputDto equipmentBroker = await _sqlConnection.QueryFirstOrDefaultAsync<EquipmentBrokerOutputDto>(query, new { zoneId });

            return equipmentBroker;
        }

        private string GetQuery()
        {
            return @"Select 
                        e.Id,
                        e.Title
                	From [Aban360].InstallationPool.EquipmentBrokerZone ez
                	Join [Aban360].InstallationPool.EquipmentBroker e
                		ON ez.EquipmentBrokerId=e.Id
                	Where ez.ZoneId=@zoneId";
        }
    }
}
