using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class GhestQueryService : AbstractBaseConnection, IGhestQueryService
    {
        public GhestQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<InstallmentRequestDataOutputDto>> Get(string stringTrackNumber, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetByTrackNumberQuery(dbName);
            IEnumerable<InstallmentRequestDataOutputDto> data = await _sqlReportConnection.QueryAsync<InstallmentRequestDataOutputDto>(query, new { stringTrackNumber });
            return data;
        }
        public async Task<InstallmentRequestDataOutputDto> Get(int id, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetByIdQuery(dbName);
            InstallmentRequestDataOutputDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<InstallmentRequestDataOutputDto>(query, new { id });
            return data;
        }

        private string GetByTrackNumberQuery(string dbName)
        {
            return $@"Select 
                    	pard Amount,
                    	mohlat DueDateJalali,
                    	sh_pard1 PaymentId
                    From [{dbName}].dbo.ghest
                    Where par_no=@stringTrackNumber";
        }
        private string GetByIdQuery(string dbName)
        {
            return $@"Select 
                    	pard Amount,
                    	mohlat DueDateJalali,
                    	sh_pard1 PaymentId
                    From [{dbName}].dbo.ghest
                    Where id=@id";
        }
    }
}
