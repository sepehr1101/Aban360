using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    internal sealed class InstallationDateAssignmentCommandService : AbstractBaseConnection, IInstallationDateAssignmentCommandService
    {
        public InstallationDateAssignmentCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Update(InstallationDateAssignmentUpdateDto updateDto)
        {
            string installationDateAssingmentQuery = GetInstallationDateAssignmentUpdateQuery();
            var @params = new
            {
                billId = updateDto.BillId,
                waterInstallationDate = updateDto.WaterInstallationDate,
                sewageInstallationDate = updateDto.SewageInstallationDate,
            };
            await _sqlReportConnection.ExecuteAsync(installationDateAssingmentQuery, @params);
        }
        private string GetInstallationDateAssignmentUpdateQuery()
        {
            return @"Update [CustomerWarehouse].dbo.Clients 
                     Set 
                     	WaterInstallDate=@waterInstallationDate , SewageInstallDate=@sewageInstallationDate
                     where 
                     	ToDayJalali IS NULL AND
                     	BillId=@billId";
        }
    }
}
