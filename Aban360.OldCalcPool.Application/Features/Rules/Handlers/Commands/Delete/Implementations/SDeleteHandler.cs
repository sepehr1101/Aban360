using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Implementations
{
    internal sealed class SDeleteHandler : AbstractBaseConnection, ISDeleteHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISQueryService _sQueryService;
        public SDeleteHandler(
            IHttpContextAccessor contextAccessor,
            ISQueryService sQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _sQueryService = sQueryService;
            _sQueryService.NotNull(nameof(sQueryService));
        }
        public async Task Handle(int id, IAppUser appUser, CancellationToken cancellationToken)
        {
            SGetDto sInfo = await _sQueryService.Get(id);
            string opLogText = string.Format(OpLogLiterals.SDeleteOpLog, id, sInfo.Olgo, sInfo.ZoneId, sInfo.FromDateJalali, sInfo.ToDateJalali);
            await ExecSql(id, appUser, opLogText);
        }
        private async Task ExecSql(int id, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    SCommandService sCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await sCommandService.Delete(id);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
    }
}
