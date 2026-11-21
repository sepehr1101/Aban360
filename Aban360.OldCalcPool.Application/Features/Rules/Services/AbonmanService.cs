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

namespace Aban360.OldCalcPool.Application.Features.Rules.Services;

public interface IAbonmanService
{
    Task CreateAsync(AbonmanCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(AbonmanUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task<AbonmanGetDto> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<AbonmanGetDto>> GetAllAsync(CancellationToken cancellationToken);
}

internal sealed class AbonmanService : AbstractBaseConnection, IAbonmanService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IValidator<AbonmanCreateDto> _createValidator;
    private readonly IAbonmanQueryService _abonmanQueryService;
    private readonly IValidator<AbonmanUpdateDto> _updateValidator;
    public AbonmanService(
        IHttpContextAccessor contextAccessor,
        IValidator<AbonmanCreateDto> validator,
        IConfiguration configuration,
        IAbonmanQueryService abonmanQueryService,
        IValidator<AbonmanUpdateDto> updateValidator) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _createValidator = validator;
        _createValidator.NotNull(nameof(validator));
        _abonmanQueryService = abonmanQueryService;
        _abonmanQueryService.NotNull(nameof(abonmanQueryService));
        _updateValidator = updateValidator;
        _updateValidator.NotNull(nameof(updateValidator));
    }

    public async Task CreateAsync(AbonmanCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await CreateInputValidate(inputDto, cancellationToken);
        string opLogText = string.Format(OpLogLiterals.AbonmanInsertOpLog, inputDto.Code, inputDto.Vaj, inputDto.Date1, inputDto.Date2);
        await CreateExecSql(inputDto, appUser, opLogText);
    }

    private async Task CreateExecSql(AbonmanCreateDto createDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                AbonmanCommandService abonmanCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await abonmanCommandService.Insert(createDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    public async Task CreateInputValidate(AbonmanCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task UpdateAsync(AbonmanUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        AbonmanGetDto previousInfo = await _abonmanQueryService.Get(inputDto.Id);
        string opLogText = string.Format(OpLogLiterals.AbonmanUpdateOpLog, previousInfo.Code, inputDto.Code, previousInfo.Vaj, inputDto.Vaj, previousInfo.Date1, inputDto.Date1, previousInfo.Date2, inputDto.Date2);
        await UpdateExecSql(inputDto, appUser, opLogText);
    }

    private async Task UpdateExecSql(AbonmanUpdateDto UpdateDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                AbonmanCommandService abonmanCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await abonmanCommandService.Update(UpdateDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    public async Task UpdateInputValidate(AbonmanUpdateDto UpdateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(UpdateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task<AbonmanGetDto> GetAsync(int id, CancellationToken cancellationToken)
    {
        AbonmanGetDto result = await _abonmanQueryService.Get(id);
        return result;
    }

    public async Task<IEnumerable<AbonmanGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<AbonmanGetDto> result = await _abonmanQueryService.Get();
        return result;
    }
}
