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
public interface INerkhService
{
    Task CreateAsync(NerkhCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(NerkhUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task<NerkhGetDto> GetAsync(int id, int nerkh, CancellationToken cancellationToken);
    Task<NerkhGetDto> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<NerkhGetDto>> GetAllAsync(int nerkh, CancellationToken cancellationToken);
    Task<IEnumerable<NerkhGetDto>> GetAllAsync(CancellationToken cancellationToken);
}

internal sealed class NerkhService : global::CriticalRecordingConnectionBase, INerkhService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IValidator<NerkhCreateDto> _createValidator;
    private readonly INerkhGetService _nerkhGetService;
    private readonly IValidator<NerkhUpdateDto> _updateValidator;
    private readonly INerkhGetAllService _nerkhGetAllService;
    public NerkhService(IHttpContextAccessor contextAccessor, IValidator<NerkhCreateDto> nerkhCreateValidator, IConfiguration configuration, INerkhGetService nerkhGetService, IValidator<NerkhUpdateDto> validator, INerkhGetAllService nerkhGetAllService) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _createValidator = nerkhCreateValidator;
        _createValidator.NotNull(nameof(_createValidator));
        _nerkhGetService = nerkhGetService;
        _nerkhGetService.NotNull(nameof(nerkhGetService));
        _updateValidator = validator;
        _updateValidator.NotNull(nameof(validator));
        _nerkhGetAllService = nerkhGetAllService;
        _nerkhGetAllService.NotNull(nameof(nerkhGetAllService));
    }

    public async Task CreateAsync(NerkhCreateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await CreateInputValidate(inputDto, cancellationToken);
        string opLogText = string.Format(OpLogLiterals.NerkhInsertOpLog, inputDto.Cod, inputDto.Vaj, inputDto.Date1, inputDto.Date2);
        await CreateExecSql(inputDto, appUser, opLogText);
    }

    private async Task CreateExecSql(NerkhCreateDto createDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                NerkhCommandService NerkhCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await NerkhCommandService.Insert(createDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task CreateInputValidate(NerkhCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task UpdateAsync(NerkhUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        NerkhGetDto previousInfo = await _nerkhGetService.Get(inputDto.Id);
        string opLogText = string.Format(OpLogLiterals.NerkhUpdateOpLog, previousInfo.Cod, inputDto.Cod, previousInfo.Vaj, inputDto.Vaj, previousInfo.Date1, inputDto.Date1, previousInfo.Date2, inputDto.Date2);
        await UpdateExecSql(inputDto, appUser, opLogText);
    }

    private async Task UpdateExecSql(NerkhUpdateDto updateDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                NerkhCommandService NerkhCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await NerkhCommandService.Update(updateDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task UpdateInputValidate(NerkhUpdateDto UpdateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(UpdateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task<NerkhGetDto> GetAsync(int id, int nerkh, CancellationToken cancellationToken)
    {
        NerkhGetDto result = await _nerkhGetService.Get(id, nerkh);
        return result;
    }

    public async Task<NerkhGetDto> GetAsync(int id, CancellationToken cancellationToken)
    {
        NerkhGetDto result = await _nerkhGetService.Get(id);
        return result;
    }

    public async Task<IEnumerable<NerkhGetDto>> GetAllAsync(int nerkh, CancellationToken cancellationToken)
    {
        IEnumerable<NerkhGetDto> result = await _nerkhGetAllService.Get(nerkh);
        return result;
    }

    public async Task<IEnumerable<NerkhGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<NerkhGetDto> result = await _nerkhGetAllService.Get();
        return result;
    }
}
