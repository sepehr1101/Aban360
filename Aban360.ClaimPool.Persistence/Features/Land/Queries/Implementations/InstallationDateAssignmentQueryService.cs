using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class InstallationDateAssignmentQueryService : AbstractBaseConnection, IInstallationDateAssignmentQueryService
    {
        public InstallationDateAssignmentQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<InstallationDateAssignmentGetDto> Get(string billId)
        {
            string installationDateAssignmentQuery = GetInstallationDateAssignmentQuery();
            InstallationDateAssignmentGetDto installationDateData = await _sqlReportConnection.QueryFirstAsync<InstallationDateAssignmentGetDto>(installationDateAssignmentQuery, new { billId });

            return installationDateData;
        }

        private string GetInstallationDateAssignmentQuery()
        {
            return @"Select
                    	c.SewageInstallDate AS SewageInstallationDate,
                    	c.WaterInstallDate AS WaterInstallationDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	c.BillId=@billId";
        }
    }
}
