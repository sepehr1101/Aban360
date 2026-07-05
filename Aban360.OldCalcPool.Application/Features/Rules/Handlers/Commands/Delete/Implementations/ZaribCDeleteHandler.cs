using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Implementations
{
    internal sealed class ZaribCDeleteHandler : AbstractBaseConnection, IZaribCDeleteHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public ZaribCDeleteHandler(
            IHttpContextAccessor contextAccessor,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));
        }
        public async Task Handle(int id, IAppUser appUser, CancellationToken cancellationToken)
        {
            string opLogText = string.Format(OpLogLiterals.ZaribCDeleteOpLog, id);
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
                    ZaribCCommandService zaribCCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await zaribCCommandService.Delete(id);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
    }
}
