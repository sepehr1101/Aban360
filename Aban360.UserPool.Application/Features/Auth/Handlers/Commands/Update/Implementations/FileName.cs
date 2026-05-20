using Aban360.Common.Db.Dapper;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations
{
    public interface IKartableAccessUpdateHandler
    {
        Task Handle(KartableAccessUpdateDto inputDto,CancellationToken cancellationToken);
    }
    internal sealed class KartableAccessUpdateHandler : AbstractBaseConnection, IKartableAccessUpdateHandler
    {
        public KartableAccessUpdateHandler(IConfiguration configuration)
            :base(configuration) 
        {
        }

        public async Task Handle(KartableAccessUpdateDto inputDto, CancellationToken cancellationToken)
        {
            using (IDbConnection connetinon = _sqlReportConnection)
            {

            }
        }
    }
}
