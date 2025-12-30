using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Commands.Implementations
{
    internal sealed class TankerWaterSetPayCommandService : AbstractBaseConnection, ITankerWaterSetPayCommandService
    {
        public TankerWaterSetPayCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Insert(TankerWaterSetPayWithZoneIdAndCustomerNumberInputDto input)
        {
            //string dbName = GetDbName(input.ZoneId);
            string dbName = "Atlas";
            string command = GetInsertCommand(dbName);


        }

        private string GetInsertCommand(string dbName)
        {
            return $@"";
        }
    }
}
