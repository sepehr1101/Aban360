using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations
{
    internal sealed class MoshtrakCommandService : AbstractBaseConnection, IMoshtrakCommandService
    {
        public MoshtrakCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Update(MoshtrkUpdateDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            //string dbName = "Atlas";
            string command = GetUpdateCommand(dbName);

            await _sqlReportConnection.ExecuteAsync(command, input);
        }

        private string GetUpdateCommand(string dbName)
        {
            return $@"Update [{dbName}].dbo.moshtrak
                    Set 
                    	noe_va=1 ,
                    	cod_enshab=1 ,
                    	enshab=1 
                    	--SiphonDiamterId?
                    Where TrackingNumber=@trackNumber";
        }
    }
}
