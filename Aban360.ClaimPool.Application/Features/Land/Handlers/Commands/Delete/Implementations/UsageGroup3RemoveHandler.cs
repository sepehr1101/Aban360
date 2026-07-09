using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Implementations
{
    internal sealed class UsageGroup3RemoveHandler : AbstractBaseConnection, IUsageGroup3RemoveHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUsageGroup3QueryService _UsageGroup3QueryService;
        public UsageGroup3RemoveHandler(
            IHttpContextAccessor contextAccessor,
            IUsageGroup3QueryService UsageGroup3QueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _UsageGroup3QueryService = UsageGroup3QueryService;
            _UsageGroup3QueryService.NotNull(nameof(UsageGroup3QueryService));
        }
        public async Task Handle(short id, IAppUser appUser, CancellationToken cancellationToken)
        {
            UsageGroup3GetDto UsageGroup3Info = await _UsageGroup3QueryService.Get(id);
            string opLogText = string.Format(OpLogLiterals.UsageGroup3DeleteOpLog, id);
            await ExecSql(id, appUser, opLogText);
        }
        private async Task ExecSql(short id, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    UsageGroup3CommandService usageGroup3CommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await usageGroup3CommandService.Remove(id);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
    }
}
