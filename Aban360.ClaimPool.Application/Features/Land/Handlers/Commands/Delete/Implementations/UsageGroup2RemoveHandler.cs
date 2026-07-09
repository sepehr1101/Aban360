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
    internal sealed class UsageGroup2RemoveHandler : AbstractBaseConnection, IUsageGroup2RemoveHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUsageGroup2QueryService _usageGroup2QueryService;
        private readonly IUsageGroup3QueryService _usageGroup3QueryService;
        public UsageGroup2RemoveHandler(
            IHttpContextAccessor contextAccessor,
            IUsageGroup2QueryService usageGroup2QueryService,
            IUsageGroup3QueryService usageGroup3QueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _usageGroup2QueryService = usageGroup2QueryService;
            _usageGroup2QueryService.NotNull(nameof(usageGroup2QueryService));

            _usageGroup3QueryService = usageGroup3QueryService;
            _usageGroup3QueryService.NotNull(nameof(usageGroup3QueryService));
        }
        public async Task Handle(short id, IAppUser appUser, CancellationToken cancellationToken)
        {
            UsageGroup2GetDto UsageGroup2Info = await _usageGroup2QueryService.Get(id);
            IEnumerable<UsageGroup3GetDto> usageGroups3Info = await _usageGroup3QueryService.GetByParrentId(id);
            IEnumerable<short> usageGroup3Ids = usageGroups3Info.Select(u2 => u2.Id).ToList();
            string opLogText = string.Format(OpLogLiterals.UsageGroup2DeleteOpLog, UsageGroup2Info.Title);

            await ExecSql(id, usageGroup3Ids, appUser, opLogText);
        }
        private async Task ExecSql(short group1Id, IEnumerable<short> group3Ids, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    UsageGroup2CommandService usageGroup2CommandService = new(connection, transaction);
                    UsageGroup3CommandService usageGroup3CommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await usageGroup2CommandService.Remove(group1Id);
                    await usageGroup3CommandService.Remove(group3Ids);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
    }
}
