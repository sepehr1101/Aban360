using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Commands.Implementations
{
    internal sealed class InstallationAndEquipmentCommandService : AbstractBaseConnection, IInstallationAndEquipmentCommandService
    {
        public InstallationAndEquipmentCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Create(InstallationAndEquipmentInputDto input)
        {
            string query = CreateQuery();
            await _sqlReportConnection.ExecuteScalarAsync(query, input);
        }

        private string CreateQuery()
        {
            return @"INSERT INTO [Aban360].CalculationPool.InstallationAndEquipment (
                        IsWater,MeterDiameterId,InstallationAmount,
                        EquipmentAmount,RegisterDateJalali,FromDateJalali,
                        ToDateJalali,RemovedDateJalali
                    )
                    VALUES (
                        @IsWater,@MeterDiameterId,@InstallationAmount,
                        @EquipmentAmount,@RegisterDateJalali,@FromDateJalali,
                        @ToDateJalali,@RemovedDateJalali);";
        }

    }
}
