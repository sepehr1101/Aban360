using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Application.Features.Apk.Handlers.Command.Create.Contracts;
using Aban360.MeterPool.Domain.Features.Apk.Commands;
using Aban360.MeterPool.Domain.Features.Apk.Queries;
using Aban360.MeterPool.Persistence.Features.Apk.Commands.Implementations;
using Aban360.MeterPool.Persistence.Features.Apk.Queries.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.MeterPool.Application.Features.Apk.Handlers.Command.Create.Implementations
{
    internal sealed class MeteApkFileInsertHandler : AbstractBaseConnection, IMeteApkFileInsertHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMeterApkInfoQueryService _meterApkFileQueryService;
        public MeteApkFileInsertHandler(
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

        public async Task Handle(ApkInfoInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto);
            ApkInfoInsertDto insertDto = await GetInsertDto(inputDto, appUser);
            string opLogText = string.Format(OpLogLiterals.MeterApkFileInsertOpLog, inputDto.Name, inputDto.Version);
            await ExecSql(insertDto, appUser, opLogText);
        }
        private async Task ExecSql(ApkInfoInsertDto insertDto, IAppUser appUser, string opLogText)
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

                    await apkInfoCommandService.Insert(insertDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    sqlTransaction.Commit();
                    sqlReportTransaction.Commit();
                }
            }
        }
        private async Task<ApkInfoInsertDto> GetInsertDto(ApkInfoInsertInputDto inputDto, IAppUser appUser)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                await inputDto.FileContent.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }

            return new ApkInfoInsertDto()
            {
                Name = inputDto.Name,
                Version = inputDto.Version,
                FileContent = fileBytes,
                Description = inputDto.Description,
                InsertedBy = appUser.UserId,
            };
        }
        private async Task Validate(ApkInfoInsertInputDto inputDto)
        {
            ApkInfo? result = await _meterApkFileQueryService.Get(inputDto.Version);
            if (result is not null && result.RemovedBy is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidMeterApkFileByDuplicateVersion);
            }
        }
    }
}
