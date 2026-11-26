using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Db70.Services;
public interface IBillReturnCauseService
{
    Task CreateAsync(BillReturnCauseCreateDto createDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(BillReturnCauseUpdateDto updateDto, IAppUser appUser, CancellationToken cancellationToken);
    Task DeleteAsync(BillReturnCauseDeleteDto deleteDto, IAppUser appUser, CancellationToken cancellationToken);
    Task<BillReturnCauseGetDto> GetAsync(SearchShortInputDto input, CancellationToken cancellationToken);
    Task<IEnumerable<BillReturnCauseGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<NumericDictionary>> GetAllDictionaryAsync(CancellationToken cancellationToken);
}

internal sealed class BillReturnCauseService : AbstractBaseConnection, IBillReturnCauseService
{
    private readonly IValidator<BillReturnCauseCreateDto> _validator;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IBillReturnCauseQueryService _billReturnCauseQueryService;
    private readonly IValidator<BillReturnCauseUpdateDto> _updateValidator;
    private readonly IValidator<BillReturnCauseDeleteDto> _deleteValidator;
    private readonly IValidator<SearchShortInputDto> _getValidator;
    public BillReturnCauseService(IValidator<BillReturnCauseCreateDto> validator, IHttpContextAccessor contextAccessor, IConfiguration configuration, IBillReturnCauseQueryService billReturnCauseQueryService, IValidator<BillReturnCauseUpdateDto> updateAsyncValidator, IValidator<BillReturnCauseDeleteDto> deleteAsyncValidator, IValidator<SearchShortInputDto> getAsyncValidator) : base(configuration)
    {
        _validator = validator;
        _validator.NotNull(nameof(_validator));
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _billReturnCauseQueryService = billReturnCauseQueryService;
        _billReturnCauseQueryService.NotNull(nameof(_billReturnCauseQueryService));
        _updateValidator = updateAsyncValidator;
        _updateValidator.NotNull(nameof(_updateValidator));
        _contextAccessor.NotNull(nameof(_contextAccessor));
        _deleteValidator = deleteAsyncValidator;
        _deleteValidator.NotNull(nameof(_deleteValidator));
        _billReturnCauseQueryService.NotNull(nameof(billReturnCauseQueryService));
        _getValidator = getAsyncValidator;
        _getValidator.NotNull(nameof(_getValidator));
    }

    public async Task CreateAsync(BillReturnCauseCreateDto createDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await CreateInputValidate(createDto, cancellationToken);
        createDto.RegisterDateTime = DateTime.Now;
        createDto.RegisterByUserId = appUser.UserId;
        string opLogText = string.Format(OpLogLiterals.BillReturnCauseInsertOpLog, createDto.Code, createDto.Title, createDto.IsInList, createDto.IsLastMeterValid, createDto.IsPartial);
        await CreateExecSql(createDto, appUser, opLogText);
    }

    private async Task CreateExecSql(BillReturnCauseCreateDto createDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                BillReturnCauseCommandService billReturnCauseCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await billReturnCauseCommandService.Create(createDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task CreateInputValidate(BillReturnCauseCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task UpdateAsync(BillReturnCauseUpdateDto updateDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(updateDto, cancellationToken);
        BillReturnCauseGetDto billReturnGetDto = await _billReturnCauseQueryService.Get(updateDto.Id);
        string opLogText = string.Format(OpLogLiterals.BillReturnCauseUpdateOpLog, updateDto.Id, billReturnGetDto.Code, updateDto.Code, billReturnGetDto.Title, updateDto.Title, billReturnGetDto.IsInList, updateDto.IsInList, billReturnGetDto.IsLastMeterValid, updateDto.IsLastMeterValid, billReturnGetDto.IsPartial, updateDto.IsPartial);
        await UpdateExecSql(updateDto, appUser, opLogText);
    }

    private async Task UpdateExecSql(BillReturnCauseUpdateDto updateDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                BillReturnCauseCommandService billReturnCauseCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await billReturnCauseCommandService.Update(updateDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task UpdateInputValidate(BillReturnCauseUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task DeleteAsync(BillReturnCauseDeleteDto deleteDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await DeleteInputValidate(deleteDto, cancellationToken);
        deleteDto.RemoveDateTime = DateTime.Now;
        deleteDto.RemoveByUserId = appUser.UserId;
        string opLogText = string.Format(OpLogLiterals.BillReturnCauseDeleteOpLog, deleteDto.Id);
        await DeleteExecSql(deleteDto, appUser, opLogText);
    }

    private async Task DeleteExecSql(BillReturnCauseDeleteDto deleteDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                BillReturnCauseCommandService billReturnCauseCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await billReturnCauseCommandService.Delete(deleteDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task DeleteInputValidate(BillReturnCauseDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var validationResult = await _deleteValidator.ValidateAsync(deleteDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task<BillReturnCauseGetDto> GetAsync(SearchShortInputDto input, CancellationToken cancellationToken)
    {
        var validationResult = await _getValidator.ValidateAsync(input, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        BillReturnCauseGetDto result = await _billReturnCauseQueryService.Get(input);
        return result;
    }

    public async Task<IEnumerable<BillReturnCauseGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<BillReturnCauseGetDto> data = await _billReturnCauseQueryService.Get();
        IEnumerable<BillReturnCauseGetDto> completeResult = data.Select(d => new BillReturnCauseGetDto() { Id = d.Id, Code = d.Code, Title = $"{d.Title} ({GetAllGetTitleCombineReturnType(d.Title, d.IsLastMeterValid)})", IsInList = d.IsInList, IsLastMeterValid = d.IsLastMeterValid, IsPartial = d.IsPartial, HasConsumption = d.HasConsumption }).ToList();
        return completeResult;
    }

    public async Task<IEnumerable<NumericDictionary>> GetAllDictionaryAsync(CancellationToken cancellationToken)
    {
        IEnumerable<NumericDictionary> result = await _billReturnCauseQueryService.GetByDictionary();
        return result;
    }

    private string GetAllGetTitleCombineReturnType(string title, bool isLastMeterValid) => isLastMeterValid ? ReportLiterals.ReturnFull : ReportLiterals.ReturnPartial;
}
