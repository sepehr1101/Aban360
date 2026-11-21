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
using Aban360.Common.Literals;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Services;
public interface IUsageGroup2Service
{
    Task InsertAsync(UsageGroup2InsertDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(UsageGroup2UpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task RemoveAsync(short id, IAppUser appUser, CancellationToken cancellationToken);
    Task<IEnumerable<UsageGroup2GetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<UsageGroup2GetDto> GetByIdAsync(short id, CancellationToken cancellationToken);
    Task<IEnumerable<UsageGroup2GetDto>> GetByGroup1IdAsync(short id, CancellationToken cancellationToken);
}

internal sealed class UsageGroup2Service : AbstractBaseConnection, IUsageGroup2Service
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUsageGroup2QueryService _usageGroup2QueryService;
    private readonly IValidator<UsageGroup2InsertDto> _validator;
    private readonly IValidator<UsageGroup2UpdateDto> _updateValidator;
    private readonly IUsageGroup3QueryService _usageGroup3QueryService;
    public UsageGroup2Service(IHttpContextAccessor contextAccessor, IUsageGroup2QueryService usageGroup2QueryService, IValidator<UsageGroup2InsertDto> validator, IConfiguration configuration, IValidator<UsageGroup2UpdateDto> updateAsyncValidator, IUsageGroup3QueryService usageGroup3QueryService) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _usageGroup2QueryService = usageGroup2QueryService;
        _usageGroup2QueryService.NotNull(nameof(usageGroup2QueryService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _updateValidator = updateAsyncValidator;
        _updateValidator.NotNull(nameof(updateAsyncValidator));
        _usageGroup3QueryService = usageGroup3QueryService;
        _usageGroup3QueryService.NotNull(nameof(usageGroup3QueryService));
    }

    public async Task InsertAsync(UsageGroup2InsertDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await InsertValidate(inputDto, cancellationToken);
        UsageGroup2GetDto? duplicateData = await _usageGroup2QueryService.Get(inputDto.Title, inputDto.Group1Id);
        if (duplicateData is not null)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidDuplicateUsageGroup);
        }

        string opLogText = string.Format(OpLogLiterals.UsageGroup2InsertOpLog, inputDto.Title);
        await InsertExecSql(inputDto, appUser, opLogText);
    }

    private async Task InsertExecSql(UsageGroup2InsertDto usageGroup2InsertDto, IAppUser appUser, string opLogText)
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
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await usageGroup2CommandService.Insert(usageGroup2InsertDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task InsertValidate(UsageGroup2InsertDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    public async Task UpdateAsync(UsageGroup2UpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateValidate(inputDto, cancellationToken);
        UsageGroup2GetDto previousInfo = await _usageGroup2QueryService.Get(inputDto.Id);
        string opLogText = string.Format(OpLogLiterals.UsageGroup2UpdateOpLog, previousInfo.Title, inputDto.Title, previousInfo.Group1Id, inputDto.Group1Id);
        await UpdateExecSql(inputDto, appUser, opLogText);
    }

    private async Task UpdateExecSql(UsageGroup2UpdateDto UsageGroup2UpdateDto, IAppUser appUser, string opLogText)
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
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await usageGroup2CommandService.Update(UsageGroup2UpdateDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task UpdateValidate(UsageGroup2UpdateDto inputDto, CancellationToken cancellationToken)
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
        UsageGroup2GetDto UsageGroup2Info = await _usageGroup2QueryService.Get(id);
        IEnumerable<UsageGroup3GetDto> usageGroups3Info = await _usageGroup3QueryService.GetByParrentId(id);
        IEnumerable<short> usageGroup3Ids = usageGroups3Info.Select(u2 => u2.Id).ToList();
        string opLogText = string.Format(OpLogLiterals.UsageGroup2DeleteOpLog, UsageGroup2Info.Title);
        await RemoveExecSql(id, usageGroup3Ids, appUser, opLogText);
    }

    private async Task RemoveExecSql(short group1Id, IEnumerable<short> group3Ids, IAppUser appUser, string opLogText)
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
                await usageGroup3CommandService.Remove(group3Ids);
                await usageGroup2CommandService.Remove(group1Id);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    public async Task<IEnumerable<UsageGroup2GetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<UsageGroup2GetDto> result = await _usageGroup2QueryService.Get();
        return result;
    }

    public async Task<UsageGroup2GetDto> GetByIdAsync(short id, CancellationToken cancellationToken)
    {
        UsageGroup2GetDto result = await _usageGroup2QueryService.Get(id);
        return result;
    }

    public async Task<IEnumerable<UsageGroup2GetDto>> GetByGroup1IdAsync(short id, CancellationToken cancellationToken)
    {
        IEnumerable<UsageGroup2GetDto> result = await _usageGroup2QueryService.GetByParentId(id);
        return result;
    }
}
