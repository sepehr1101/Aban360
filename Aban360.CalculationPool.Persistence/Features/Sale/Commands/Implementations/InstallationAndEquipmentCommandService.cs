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
        public async Task Update(InstallationAndEquipmentUpdateDto input)
        {
            string query = UpdateQuery();
            await _sqlReportConnection.ExecuteScalarAsync(query, input);
        }
        public async Task Delete(DeleteDto input)
        {
            string query = DeleteQuery();
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

        private string UpdateQuery()
        {
            return @"UPDATE [Aban360].CalculationPool.InstallationAndEquipment
                    SET
                        IsWater            = @IsWater,
                        MeterDiameterId    = @MeterDiameterId,
                        InstallationAmount = @InstallationAmount,
                        EquipmentAmount    = @EquipmentAmount,
                        RegisterDateJalali = @RegisterDateJalali,
                        FromDateJalali     = @FromDateJalali,
                        ToDateJalali       = @ToDateJalali
                    WHERE
                        Id = @Id;";
        }

        private string DeleteQuery()
        {
            return @"UPDATE [Aban360].CalculationPool.InstallationAndEquipment
                    SET
                        RemovedDateJalali  = @RemovedDateJalali
                    WHERE
                        Id = @Id;";
        }
    }
}
