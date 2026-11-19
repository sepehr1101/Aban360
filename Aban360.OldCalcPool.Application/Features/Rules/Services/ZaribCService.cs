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

public interface IZaribCService
{
    Task CreateAsync(ZaribCCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(ZaribCUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task DeleteAsync(int id, IAppUser appUser, CancellationToken cancellationToken);
    Task<ZaribCQueryDto> GetAsync(string fromDateJalali, string toDateJalali, CancellationToken cancellationToken);
    Task<IEnumerable<ZaribCQueryDto>> GetAllAsync(CancellationToken cancellationToken);
}

internal sealed class ZaribCService : AbstractBaseConnection, IZaribCService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IValidator<ZaribCCreateDto> _createValidator;
    private readonly IZaribCQueryService _zaribCQueryService;
    private readonly IValidator<ZaribCUpdateDto> _updateValidator;
    public ZaribCService(
        IHttpContextAccessor contextAccessor,
        IValidator<ZaribCCreateDto> validator,
        IConfiguration configuration,
        IZaribCQueryService zaribCQueryService,
        IValidator<ZaribCUpdateDto> updateValidator) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _createValidator = validator;
        _createValidator.NotNull(nameof(validator));
        _zaribCQueryService = zaribCQueryService;
        _zaribCQueryService.NotNull(nameof(zaribCQueryService));
        _updateValidator = updateValidator;
        _updateValidator.NotNull(nameof(_updateValidator));
    }

    public async Task CreateAsync(ZaribCCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await CreateInputValidate(inputDto, cancellationToken);
        string opLogText = string.Format(OpLogLiterals.ZaribCInsertOpLog, inputDto.C, inputDto.FromDateJalali, inputDto.ToDateJalali);
        await CreateExecSql(inputDto, appUser, opLogText);
    }

    private async Task CreateExecSql(ZaribCCreateDto createDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                ZaribCCommandService zaribCCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await zaribCCommandService.Insert(createDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task CreateInputValidate(ZaribCCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task UpdateAsync(ZaribCUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        ZaribCQueryDto zaribCInfo = await _zaribCQueryService.Get(inputDto.Id);
        string opLogText = string.Format(OpLogLiterals.ZaribCUpdateOpLog, zaribCInfo.C, inputDto.C, zaribCInfo.FromDateJalali, inputDto.FromDateJalali, zaribCInfo.ToDateJalali, inputDto.ToDateJalali);
        await UpdateExecSql(inputDto, appUser, opLogText);
    }

    private async Task UpdateExecSql(ZaribCUpdateDto updateDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                ZaribCCommandService zaribCCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await zaribCCommandService.Update(updateDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task UpdateInputValidate(ZaribCUpdateDto UpdateDto, CancellationToken cancellationToken)
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
        string opLogText = string.Format(OpLogLiterals.ZaribCDeleteOpLog, id);
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
                ZaribCCommandService zaribCCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await zaribCCommandService.Delete(id);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    public async Task<ZaribCQueryDto> GetAsync(string fromDateJalali, string toDateJalali, CancellationToken cancellationToken)
    {
        ZaribCQueryDto result = await _zaribCQueryService.Get(fromDateJalali, toDateJalali);
        return result;
    }

    public async Task<IEnumerable<ZaribCQueryDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<ZaribCQueryDto> result = await _zaribCQueryService.GetZaribC();
        return result;
    }
}
