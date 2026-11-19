using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace SqlAfterCritical10.Aban360.OldCalcPool.Application.Features.Rules.Services;
public interface ISService
{
    Task CreateAsync(SCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(SUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task DeleteAsync(int id, IAppUser appUser, CancellationToken cancellationToken);
    Task<IEnumerable<SGetDto>> GetAsync(string fromDateJalali, string toDateJalali, CancellationToken cancellationToken);
    Task<IEnumerable<SGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<SGetDto> GetByIdAsync(int id, CancellationToken cancellationToken);
}

internal sealed class SService : global::CriticalRecordingConnectionBase, ISService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IValidator<SCreateDto> _createValidator;
    private readonly ISQueryService _sQueryService;
    private readonly IValidator<SUpdateDto> _updateValidator;
    public SService(IHttpContextAccessor contextAccessor, IValidator<SCreateDto> validator, IConfiguration configuration, ISQueryService sQueryService, IValidator<SUpdateDto> updateValidator) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _createValidator = validator;
        _createValidator.NotNull(nameof(_createValidator));
        _sQueryService = sQueryService;
        _sQueryService.NotNull(nameof(sQueryService));
        _updateValidator = updateValidator;
        _updateValidator.NotNull(nameof(_updateValidator));
    }

    public async Task CreateAsync(SCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await CreateInputValidate(inputDto, cancellationToken);
        string opLogText = string.Format(OpLogLiterals.SInsertOpLog, inputDto.Olgo, inputDto.ZoneId, inputDto.FromDateJalali, inputDto.ToDateJalali);
        await CreateExecSql(inputDto, appUser, opLogText);
    }

    private async Task CreateExecSql(SCreateDto createDto, IAppUser appUser, string opLogText)
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
                await sCommandService.Insert(createDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task CreateInputValidate(SCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task UpdateAsync(SUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        SGetDto previousInfo = await _sQueryService.Get(inputDto.Id);
        string opLogText = string.Format(OpLogLiterals.SUpdateOpLog, previousInfo.Olgo, inputDto.Olgo, previousInfo.ZoneId, inputDto.ZoneId, previousInfo.FromDateJalali, inputDto.FromDateJalali, previousInfo.ToDateJalali, inputDto.ToDateJalali);
        await UpdateExecSql(inputDto, appUser, opLogText);
    }

    private async Task UpdateExecSql(SUpdateDto updateDto, IAppUser appUser, string opLogText)
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
                await sCommandService.Update(updateDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task UpdateInputValidate(SUpdateDto UpdateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(UpdateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task DeleteAsync(int id, IAppUser appUser, CancellationToken cancellationToken)
    {
        SGetDto sInfo = await _sQueryService.Get(id);
        string opLogText = string.Format(OpLogLiterals.SDeleteOpLog, id, sInfo.Olgo, sInfo.ZoneId, sInfo.FromDateJalali, sInfo.ToDateJalali);
        await DeleteExecSql(id, appUser, opLogText);
    }

    private async Task DeleteExecSql(int id, IAppUser appUser, string opLogText)
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

    public async Task<IEnumerable<SGetDto>> GetAsync(string fromDateJalali, string toDateJalali, CancellationToken cancellationToken)
    {
        IEnumerable<SGetDto> result = await _sQueryService.Get(fromDateJalali, toDateJalali);
        return result;
    }

    public async Task<IEnumerable<SGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<SGetDto> result = await _sQueryService.Get();
        return result;
    }

    public async Task<SGetDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        SGetDto result = await _sQueryService.Get(id);
        return result;
    }
}
