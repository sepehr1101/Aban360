using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Domain.Features.Apk.Commands;
using Aban360.MeterPool.Domain.Features.Apk.Queries;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Apk.Commands.Implementations;
using Aban360.MeterPool.Persistence.Features.Apk.Queries.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace SqlAfterAuxiliary10.Aban360.MeterPool.Application.Features.Apk.Services;
public interface IMeterApkFileService
{
    Task CreateAsync(ApkInfoInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task DeleteAsync(short id, IAppUser appUser, CancellationToken cancellationToken);
    Task SetActiveAsync(int id, IAppUser appUser, CancellationToken cancellationToken);
    Task<IEnumerable<ApkInfoGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<ApkInfoGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ApkInfoGetDto?> GetLatestAsync(CancellationToken cancellationToken);
    Task<MeterApkValidateOutputDto> ValidateVersionAsync(string version, CancellationToken cancellationToken);
    Task<byte[]> DownloadAsync(short id, CancellationToken cancellationToken);
}

internal sealed class MeterApkFileService : global::AuxiliaryRecordingConnectionBase, IMeterApkFileService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IMeterApkInfoQueryService _meterApkFileQueryService;
    public MeterApkFileService(IHttpContextAccessor contextAccessor, IMeterApkInfoQueryService meterApkFileQueryService, IConfiguration configuration) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _meterApkFileQueryService = meterApkFileQueryService;
        _meterApkFileQueryService.NotNull(nameof(meterApkFileQueryService));
    }

    public async Task CreateAsync(ApkInfoInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await CreateValidate(inputDto);
        ApkInfoInsertDto insertDto = await CreateGetInsertDto(inputDto, appUser);
        string opLogText = string.Format(OpLogLiterals.MeterApkFileInsertOpLog, inputDto.Name, inputDto.Version);
        await CreateExecSql(insertDto, appUser, opLogText);
    }

    private async Task CreateExecSql(ApkInfoInsertDto insertDto, IAppUser appUser, string opLogText)
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
                await apkInfoCommandService.Update(false);
                await apkInfoCommandService.Insert(insertDto);
                await opLogCommandService.Insert(opLogText, appUser);
                sqlTransaction.Commit();
                sqlReportTransaction.Commit();
            }
        }
    }

    private async Task<ApkInfoInsertDto> CreateGetInsertDto(ApkInfoInsertInputDto inputDto, IAppUser appUser)
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
            IsActive = true
        };
    }

    private async Task CreateValidate(ApkInfoInsertInputDto inputDto)
    {
        ApkInfo? result = await _meterApkFileQueryService.GetValid(inputDto.Version);
        if (result is not null && result.RemovedBy is null)
        {
            throw new ReadingException(ExceptionLiterals.InvalidMeterApkFileByDuplicateVersion);
        }
    }

    public async Task DeleteAsync(short id, IAppUser appUser, CancellationToken cancellationToken)
    {
        ApkInfoGetDto? apkInfo = await _meterApkFileQueryService.GetValid(id);
        if (apkInfo is null)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveMeterApkFile);
        }

        ApkInfo? latestValidVersionInfo = await _meterApkFileQueryService.GetLatestValidVersion();
        int? latestValidId = apkInfo.IsActive ? latestValidVersionInfo.Id : null;
        ApkInfoRemoveDto removedDto = new(id, appUser.UserId);
        string opLogText = string.Format(OpLogLiterals.MeterApkFileRemoveOpLog, id);
        await DeleteExecSql(removedDto, latestValidId, appUser, opLogText);
    }

    private async Task DeleteExecSql(ApkInfoRemoveDto RemoveDto, int? latestValidId, IAppUser appUser, string opLogText)
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
                if (latestValidId != null)
                {
                    await apkInfoCommandService.Update(new ApkInfoIsActiveUpdateDto(latestValidId.Value, true));
                    await apkInfoCommandService.Update(new ApkInfoIsActiveUpdateDto(RemoveDto.Id, false));
                }

                await opLogCommandService.Insert(opLogText, appUser);
                sqlTransaction.Commit();
                sqlReportTransaction.Commit();
            }
        }
    }

    public async Task SetActiveAsync(int id, IAppUser appUser, CancellationToken cancellationToken)
    {
        ApkInfoIsActiveUpdateDto updateDto = new(id, true);
        string opLogText = string.Format(OpLogLiterals.MeterApkFileUdpateIsActiveOpLog, id);
        await SetActiveExecSql(updateDto, appUser, opLogText);
    }

    private async Task SetActiveExecSql(ApkInfoIsActiveUpdateDto updateDto, IAppUser appUser, string opLogText)
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
                await apkInfoCommandService.Update(false);
                await apkInfoCommandService.Update(updateDto);
                await opLogCommandService.Insert(opLogText, appUser);
                sqlTransaction.Commit();
                sqlReportTransaction.Commit();
            }
        }
    }

    public async Task<IEnumerable<ApkInfoGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<ApkInfoGetDto> result = await _meterApkFileQueryService.GetValid();
        return result;
    }

    public async Task<ApkInfoGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        ApkInfoGetDto result = await _meterApkFileQueryService.GetValid(id);
        return result;
    }

    public async Task<ApkInfoGetDto?> GetLatestAsync(CancellationToken cancellationToken)
    {
        ApkInfoGetDto? result = await _meterApkFileQueryService.GetLatestVersion();
        if (result is null || result.FileContent is null)
        {
            throw new ReadingException(ExceptionLiterals.NotFoundMeterApkFileToDownload);
        }

        return result;
    }

    public async Task<MeterApkValidateOutputDto> ValidateVersionAsync(string version, CancellationToken cancellationToken)
    {
        ApkInfo? apkInfo = await _meterApkFileQueryService.GetValid(version);
        string latestValidVersion = (await _meterApkFileQueryService.GetLatestVersion())?.Version ?? string.Empty;
        //if (apkInfo is null)
        //{
        ////throw new ReadingException(ExceptionLiterals.NotFoundMeterApkFileVersion);
        //}
        if (apkInfo is null || !apkInfo.IsActive)
        {
            return new MeterApkValidateOutputDto(false, latestValidVersion, version);
        }

        return new MeterApkValidateOutputDto(true, latestValidVersion, version);
    }

    public async Task<byte[]> DownloadAsync(short id, CancellationToken cancellationToken)
    {
        byte[] result = await _meterApkFileQueryService.GetFile(id);
        return result;
    }
}
