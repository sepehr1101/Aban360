using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace SqlAfterManagement20.Aban360.ClaimPool.Application.Features.Land.Services;
public interface IUsageGroup3Service
{
    Task InsertAsync(UsageGroup3InsertDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(UsageGroup3UpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task RemoveAsync(short id, IAppUser appUser, CancellationToken cancellationToken);
    Task<IEnumerable<UsageGroup3GetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<UsageGroup3GetDto> GetByIdAsync(short id, CancellationToken cancellationToken);
}

internal sealed class UsageGroup3Service : global::ManagementRecordingConnectionBase, IUsageGroup3Service
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IValidator<UsageGroup3InsertDto> _validator;
    private readonly IUsageGroup3QueryService _usageGroup3QueryService;
    private readonly IValidator<UsageGroup3UpdateDto> _updateValidator;
    public UsageGroup3Service(IHttpContextAccessor contextAccessor, IValidator<UsageGroup3InsertDto> validator, IConfiguration configuration, IUsageGroup3QueryService usageGroup3QueryService, IValidator<UsageGroup3UpdateDto> updateAsyncValidator) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _usageGroup3QueryService = usageGroup3QueryService;
        _usageGroup3QueryService.NotNull(nameof(usageGroup3QueryService));
        _updateValidator = updateAsyncValidator;
        _updateValidator.NotNull(nameof(updateAsyncValidator));
    }

    public async Task InsertAsync(UsageGroup3InsertDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await InsertValidate(inputDto, cancellationToken);
        await InsertExecSql(inputDto, appUser);
    }

    private async Task InsertExecSql(UsageGroup3InsertDto usageGroup3InsertDto, IAppUser appUser)
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
                int removedEffectedRecord = await usageGroup3CommandService.RemoveByUsageGroup2(usageGroup3InsertDto.Group2Id, false);
                int insertRecordCount = await usageGroup3CommandService.Insert(usageGroup3InsertDto);
                string insertOpLogText = string.Format(OpLogLiterals.UsageGroup3InsertOpLog, usageGroup3InsertDto.Group2Id, insertRecordCount);
                string removeOpLogText = string.Format(OpLogLiterals.UsageGroup3DeleteListOpLog, usageGroup3InsertDto.Group2Id, removedEffectedRecord);
                await opLogCommandService.Insert(removeOpLogText, appUser);
                await opLogCommandService.Insert(insertOpLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task InsertValidate(UsageGroup3InsertDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    public async Task UpdateAsync(UsageGroup3UpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateValidate(inputDto, cancellationToken);
        UsageGroup3GetDto previousInfo = await _usageGroup3QueryService.Get(inputDto.Id);
        string opLogText = string.Format(OpLogLiterals.UsageGroup3UpdateOpLog, previousInfo.UsageId, inputDto.UsageId, previousInfo.Group2Id, inputDto.Group2Id);
        await UpdateExecSql(inputDto, appUser, opLogText);
    }

    private async Task UpdateExecSql(UsageGroup3UpdateDto UsageGroup3UpdateDto, IAppUser appUser, string opLogText)
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
                await usageGroup3CommandService.Update(UsageGroup3UpdateDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task UpdateValidate(UsageGroup3UpdateDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    public async Task RemoveAsync(short id, IAppUser appUser, CancellationToken cancellationToken)
    {
        UsageGroup3GetDto UsageGroup3Info = await _usageGroup3QueryService.Get(id);
        string opLogText = string.Format(OpLogLiterals.UsageGroup3DeleteOpLog, id);
        await RemoveExecSql(id, appUser, opLogText);
    }

    private async Task RemoveExecSql(short id, IAppUser appUser, string opLogText)
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

    public async Task<IEnumerable<UsageGroup3GetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<UsageGroup3GetDto> result = await _usageGroup3QueryService.Get();
        return result;
    }

    public async Task<UsageGroup3GetDto> GetByIdAsync(short id, CancellationToken cancellationToken)
    {
        UsageGroup3GetDto result = await _usageGroup3QueryService.Get(id);
        return result;
    }
}
