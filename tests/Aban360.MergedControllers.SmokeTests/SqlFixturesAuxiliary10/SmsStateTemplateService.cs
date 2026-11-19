using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace SqlAfterAuxiliary10.Aban360.CalculationPool.Application.Features.MeterReading.Services;
public interface ISmsStateTemplateService
{
    Task CreateAsync(SmsStateTemplateInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task DeleteAsync(short id, IAppUser appUser, CancellationToken cancellationToken);
    Task<SmsStateTemplateGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<IEnumerable<SmsStateTemplateGetDto>> GetAllAsync(CancellationToken cancellationToken);
}

internal sealed class SmsStateTemplateService : global::AuxiliaryRecordingConnectionBase, ISmsStateTemplateService
{
    private readonly IValidator<SmsStateTemplateInsertInputDto> _createValidator;
    private readonly ISmsStateTemplateQueryService _meterSmsStateService;
    public SmsStateTemplateService(IValidator<SmsStateTemplateInsertInputDto> validator, IConfiguration configuration, ISmsStateTemplateQueryService meterSmsStateService) : base(configuration)
    {
        _createValidator = validator;
        _createValidator.NotNull(nameof(validator));
        _meterSmsStateService = meterSmsStateService;
        _meterSmsStateService.NotNull(nameof(meterSmsStateService));
    }

    public async Task CreateAsync(SmsStateTemplateInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await CreateInputValidate(inputDto, cancellationToken);
        SmsStateTemplateInsertDto insertDto = CreateGetInsertDto(inputDto, appUser);
        await CreateExecSql(insertDto);
    }

    private async Task CreateExecSql(SmsStateTemplateInsertDto inputDto)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                SmsStateTemplateCommandService meterSmsStateCommandService = new(connection, transaction);
                await meterSmsStateCommandService.Insert(inputDto);
                transaction.Commit();
            }
        }
    }

    private async Task CreateInputValidate(SmsStateTemplateInsertInputDto input, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(input, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    private SmsStateTemplateInsertDto CreateGetInsertDto(SmsStateTemplateInsertInputDto inputDto, IAppUser appUser)
    {
        return new SmsStateTemplateInsertDto()
        {
            GroupId = inputDto.GroupId,
            SmsTypeId = inputDto.SmsTypeId,
            StepOrder = inputDto.StepOrder,
            SmsText = inputDto.SmsText,
            DueDay = inputDto.DueDay,
            Description = inputDto.Description,
            InsertBy = appUser.UserId,
        };
    }

    public async Task DeleteAsync(short id, IAppUser appUser, CancellationToken cancellationToken)
    {
        SmsStateTemplateRemoveDto removeDto = new(id, appUser.UserId);
        await DeleteExecSql(removeDto);
    }

    private async Task DeleteExecSql(SmsStateTemplateRemoveDto inputDto)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                SmsStateTemplateCommandService meterSmsStateCommandService = new(connection, transaction);
                await meterSmsStateCommandService.Delete(inputDto);
                transaction.Commit();
            }
        }
    }

    public async Task<SmsStateTemplateGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        SmsStateTemplateGetDto meterSmsState = await _meterSmsStateService.Get(id);
        return meterSmsState;
    }

    public async Task<IEnumerable<SmsStateTemplateGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<SmsStateTemplateGetDto> meterSmsState = await _meterSmsStateService.Get(false);
        return meterSmsState;
    }
}
