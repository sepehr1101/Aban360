using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations;
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
public interface IUsageGroup1Service
{
    Task InsertAsync(UsageGroup1InsertDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task<UsageGroup1DuplicateInsertOutputDto> DuplicateInsertAsync(UsageGroup1DuplicateInsetInputDto inputDto, CancellationToken cancellationToken);
    Task UpdateAsync(UsageGroup1UpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task RemoveAsync(short id, IAppUser appUser, CancellationToken cancellationToken);
    Task<IEnumerable<UsageGroup1GetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<UsageGroup1GetDto> GetByIdAsync(int id, CancellationToken cancellationToken);
}

internal sealed class UsageGroup1Service : AbstractBaseConnection, IUsageGroup1Service
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUsageGroup1QueryService _usageGroup1QueryService;
    private readonly IValidator<UsageGroup1InsertDto> _validator;
    private readonly IUsageGroup2QueryService _UsageGroup2QueryService;
    private readonly IUsageGroup3QueryService _UsageGroup3QueryService;
    private readonly IValidator<UsageGroup1UpdateDto> _updateValidator;
    public UsageGroup1Service(IHttpContextAccessor contextAccessor, IUsageGroup1QueryService usageGroup1QueryService, IValidator<UsageGroup1InsertDto> validator, IConfiguration configuration, IUsageGroup2QueryService UsageGroup2QueryService, IUsageGroup3QueryService UsageGroup3QueryService, IValidator<UsageGroup1UpdateDto> updateAsyncValidator) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _usageGroup1QueryService = usageGroup1QueryService;
        _usageGroup1QueryService.NotNull(nameof(usageGroup1QueryService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _UsageGroup2QueryService = UsageGroup2QueryService;
        _UsageGroup2QueryService.NotNull(nameof(UsageGroup2QueryService));
        _UsageGroup3QueryService = UsageGroup3QueryService;
        _UsageGroup3QueryService.NotNull(nameof(UsageGroup3QueryService));
        _updateValidator = updateAsyncValidator;
        _updateValidator.NotNull(nameof(updateAsyncValidator));
    }

    public async Task InsertAsync(UsageGroup1InsertDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await InsertValidate(inputDto, cancellationToken);
        UsageGroup1GetDto? duplicateData = await _usageGroup1QueryService.Get(inputDto.Title);
        if (duplicateData is not null)
        {
            throw new InvalidTrackingException(ExceptionLiterals.InvalidDuplicateUsageGroup);
        }

        string opLogText = string.Format(OpLogLiterals.UsageGroup1InsertOpLog, inputDto.Title);
        await InsertExecSql(inputDto, appUser, opLogText);
    }

    private async Task InsertExecSql(UsageGroup1InsertDto usageGroup1InsertDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                UsageGroup1CommandService usageGroup1CommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await usageGroup1CommandService.Insert(usageGroup1InsertDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task InsertValidate(UsageGroup1InsertDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    public async Task<UsageGroup1DuplicateInsertOutputDto> DuplicateInsertAsync(UsageGroup1DuplicateInsetInputDto inputDto, CancellationToken cancellationToken)
    {
        UsageGroup1GetDto usageGroup1 = await _usageGroup1QueryService.Get(inputDto.UsageGroup1Id);
        IEnumerable<UsageGroup2GetDto> usageGroup2s = await _UsageGroup2QueryService.GetByParentId(inputDto.UsageGroup1Id);
        IEnumerable<UsageGroup3GetDto> usageGroup3s = await _UsageGroup3QueryService.GetByParrentIds(usageGroup2s.Select(tg => tg.Id));
        if (!inputDto.IsConfirm)
        {
            return new UsageGroup1DuplicateInsertOutputDto(1, usageGroup2s?.Count() ?? 0, usageGroup3s?.Count() ?? 0, inputDto.UsageGroup1Id, inputDto.IsConfirm);
        }

        UsageGroup1DuplicateInsertDto UsageGroup1InsertDto = new(inputDto.UsageGroup1Id, inputDto.UsageGroup1Title);
        await DuplicateInsertExecSql(inputDto.UsageGroup1Id, UsageGroup1InsertDto);
        return new UsageGroup1DuplicateInsertOutputDto(1, usageGroup2s?.Count() ?? 0, usageGroup3s?.Count() ?? 0, inputDto.UsageGroup1Id, inputDto.IsConfirm);
    }

    private async Task DuplicateInsertExecSql(short previousGroup1Id, UsageGroup1DuplicateInsertDto UsageGroup1InsertDto)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                UsageGroup1CommandService usageGroup1CommandService = new(connection, transaction);
                UsageGroup2CommandService usageGroup2CommandService = new(connection, transaction);
                UsageGroup3CommandService usageGroup3CommandService = new(connection, transaction);
                short newUsageGroup1Id = await usageGroup1CommandService.InsertDuplicate(UsageGroup1InsertDto);
                IEnumerable<UsageGroup2GetDto> prevoiusGroup2 = await _UsageGroup2QueryService.GetByParentId(previousGroup1Id);
                foreach (var pre2 in prevoiusGroup2)
                {
                    short newGroup2Id = await usageGroup2CommandService.InsertDuplicateById(pre2.Id, newUsageGroup1Id);
                    await usageGroup3CommandService.InsertByParentId(pre2.Id, newGroup2Id);
                }

                transaction.Commit();
            }
        }
    }

    public async Task UpdateAsync(UsageGroup1UpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateValidate(inputDto, cancellationToken);
        UsageGroup1GetDto previousInfo = await _usageGroup1QueryService.Get(inputDto.Id);
        string opLogText = string.Format(OpLogLiterals.UsageGroup1UpdateOpLog, previousInfo.Title, inputDto.Title);
        await UpdateExecSql(inputDto, appUser, opLogText);
    }

    private async Task UpdateExecSql(UsageGroup1UpdateDto UsageGroup1UpdateDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                UsageGroup1CommandService usageGroup1CommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await usageGroup1CommandService.Update(UsageGroup1UpdateDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task UpdateValidate(UsageGroup1UpdateDto inputDto, CancellationToken cancellationToken)
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
        UsageGroup1GetDto UsageGroup1Info = await _usageGroup1QueryService.Get(id);
        IEnumerable<UsageGroup2GetDto> usageGroups2Info = await _UsageGroup2QueryService.GetByParentId(id);
        IEnumerable<short> usageGroup2Ids = usageGroups2Info.Select(u2 => u2.Id).ToList();
        IEnumerable<UsageGroup3GetDto> usageGroups3Info = await _UsageGroup3QueryService.GetByParrentIds(usageGroup2Ids);
        IEnumerable<short> usageGroup3Ids = usageGroups3Info.Select(u2 => u2.Id).ToList();
        string opLogText = string.Format(OpLogLiterals.UsageGroup1DeleteOpLog, UsageGroup1Info.Title);
        await RemoveExecSql(id, usageGroup2Ids, usageGroup3Ids, appUser, opLogText);
    }

    private async Task RemoveExecSql(short group1Id, IEnumerable<short> group2Ids, IEnumerable<short> group3Ids, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                UsageGroup1CommandService usageGroup1CommandService = new(connection, transaction);
                UsageGroup2CommandService usageGroup2CommandService = new(connection, transaction);
                UsageGroup3CommandService usageGroup3CommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await usageGroup3CommandService.Remove(group3Ids);
                await usageGroup2CommandService.Remove(group2Ids);
                await usageGroup1CommandService.Remove(group1Id);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    public async Task<IEnumerable<UsageGroup1GetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<UsageGroup1GetDto> result = await _usageGroup1QueryService.Get();
        return result;
    }

    public async Task<UsageGroup1GetDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        UsageGroup1GetDto result = await _usageGroup1QueryService.Get(id);
        return result;
    }
}
