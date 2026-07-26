using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Apk.Handlers.Command.Delete.Contracts;
using Aban360.MeterPool.Domain.Features.Apk.Commands;
using Aban360.MeterPool.Persistence.Features.Apk.Commands.Implementations;
using Aban360.MeterPool.Persistence.Features.Apk.Queries.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.MeterPool.Application.Features.Apk.Handlers.Command.Delete.Implementations
{
    internal sealed class MeteApkFileRemoveHandler : AbstractBaseConnection, IMeteApkFileRemoveHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMeterApkInfoQueryService _meterApkFileQueryService;
        public MeteApkFileRemoveHandler(
            IHttpContextAccessor contextAccessor,
            IMeterApkInfoQueryService meterApkFileQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _meterApkFileQueryService = meterApkFileQueryService;
            _meterApkFileQueryService.NotNull(nameof(meterApkFileQueryService));
        }

        public async Task Handle(short id, IAppUser appUser, CancellationToken cancellationToken)
        {
            ApkInfoRemoveDto removedDto = new(id, appUser.UserId);
            string opLogText = string.Format(OpLogLiterals.MeterApkFileRemoveOpLog, id);
            await ExecSql(removedDto, appUser, opLogText);
        }
        private async Task ExecSql(ApkInfoRemoveDto RemoveDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection sqlConnection = _sqlConnection)
            {
                IDbConnection sqlReportConnection = _sqlReportConnection;
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }
                if (sqlReportConnection.State != ConnectionState.Open)
                {
                    sqlReportConnection.Open();
                }
                using (IDbTransaction sqlTransaction = sqlConnection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    IDbTransaction sqlReportTransaction = sqlReportConnection.BeginTransaction(IsolationLevel.ReadCommitted);

                    MeterApkInfoCommandService apkInfoCommandService = new(sqlConnection, sqlTransaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, sqlReportConnection, sqlReportTransaction);

                    await apkInfoCommandService.Remove(RemoveDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    sqlTransaction.Commit();
                    sqlReportTransaction.Commit();
                }
            }
        }
    }
}
