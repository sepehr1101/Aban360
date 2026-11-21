using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Services;
public interface IMeterReadingFileRemovalService
{
    Task MeterReadingFileRemoveAsync(int id, IAppUser appUser, CancellationToken cancellation);
}

internal sealed class MeterReadingFileRemovalService : AbstractBaseConnection, IMeterReadingFileRemovalService
{
    public MeterReadingFileRemovalService(IHttpContextAccessor httpContextAccessor, IMeterFlowQueryService meterFlowQueryService, ICommonZoneService commonZoneService, IT51QueryService t51QueryService, IConfiguration configuration) : base(configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpContextAccessor.NotNull(nameof(httpContextAccessor));
        _meterFlowQueryService = meterFlowQueryService;
        _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));
        _commonZoneService = commonZoneService;
        _commonZoneService.NotNull(nameof(commonZoneService));
        _t51QueryService = t51QueryService;
        _t51QueryService.NotNull(nameof(t51QueryService));
    }

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMeterFlowQueryService _meterFlowQueryService;
    private readonly ICommonZoneService _commonZoneService;
    private readonly IT51QueryService _t51QueryService;
    static MeterFlowStepEnum[] _allowedRemoveFileStep =
    {
        MeterFlowStepEnum.Imported,
        MeterFlowStepEnum.Calculated,
        MeterFlowStepEnum.ConsumptionChecked
    };
    public async Task MeterReadingFileRemoveAsync(int id, IAppUser appUser, CancellationToken cancellation)
    {
        MeterFlowGetDto meterFlowInfo = await MeterReadingFileRemoveValidate(id);
        int firstFlowId = await _meterFlowQueryService.GetFirstFlowId(id);
        string newFileName = $"{meterFlowInfo.FileName}_{Guid.NewGuid()}";
        await _commonZoneService.IsUserInZone(appUser, meterFlowInfo?.ZoneId ?? 0);
        NumericDictionary zoneInfo = await _t51QueryService.Get(meterFlowInfo?.ZoneId ?? 0, false);
        MeterFlowDeleteDto meterFlowRemoveDto = new(id, appUser.UserId, DateTime.Now);
        MeterReadingDetailDeleteDto meterReadingDetailRemoveDto = new(firstFlowId, appUser.UserId, DateTime.Now, MeterReadingDetailRemovedType.Removed);
        string opLogText = string.Format(OpLogLiterals.MeterFlowRemoveOpLog, meterFlowInfo.FileName, newFileName, zoneInfo.Title);
        await MeterReadingFileRemoveExecSql(meterFlowRemoveDto, meterReadingDetailRemoveDto, appUser, opLogText, meterFlowInfo.FileName, newFileName);
    }

    private async Task MeterReadingFileRemoveExecSql(MeterFlowDeleteDto meterFlowRemoveDto, MeterReadingDetailDeleteDto meterReadingDetailRemoveDto, IAppUser appUser, string opLogText, string previousFileName, string newFileName)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                MeterFlowCommandService meterFlowCommand = new(connection, transaction);
                MeterReadingDetailCommandService meterReadingDetailService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommand = new(_httpContextAccessor, connection, transaction);
                await meterFlowCommand.Delete(meterFlowRemoveDto);
                await meterFlowCommand.Update(previousFileName, newFileName);
                await meterReadingDetailService.DeleteByFlowImportedId(meterReadingDetailRemoveDto);
                await opLogCommand.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task<MeterFlowGetDto> MeterReadingFileRemoveValidate(int id)
    {
        MeterFlowGetDto meterFlowInfo = await _meterFlowQueryService.Get(id);
        if (meterFlowInfo.RemovedDateTime is not null)
        {
            throw new ReadingException(ExceptionLiterals.InvalidRemoveFile);
        }

        bool isInValidStep = _allowedRemoveFileStep.Contains(meterFlowInfo.MeterFlowStepId);
        if (!isInValidStep)
        {
            throw new ReadingException(ExceptionLiterals.InvalidRemoveFinishedFile);
        }

        return meterFlowInfo;
    }
}
