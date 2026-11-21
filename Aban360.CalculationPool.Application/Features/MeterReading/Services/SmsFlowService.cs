using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Services;

public interface ISmsFlowService
{
    Task CreateAsync(SmsFlowInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(SmsFlowUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task<SmsFlowGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<IEnumerable<SmsFlowGetDto>> GetAllAsync(CancellationToken cancellationToken);
}

internal sealed class SmsFlowService : AbstractBaseConnection, ISmsFlowService
{
    private readonly IValidator<SmsFlowInsertInputDto> _createValidator;
    private readonly IValidator<SmsFlowUpdateDto> _updateValidator;
    private readonly ISmsFlowQueryService _meterSmsFlowService;
    public SmsFlowService(
        IValidator<SmsFlowInsertInputDto> validator,
        IConfiguration configuration,
        IValidator<SmsFlowUpdateDto> updateValidator,
        ISmsFlowQueryService meterSmsFlowService) : base(configuration)
    {
        _createValidator = validator;
        _createValidator.NotNull(nameof(validator));
        _updateValidator = updateValidator;
        _updateValidator.NotNull(nameof(updateValidator));
        _meterSmsFlowService = meterSmsFlowService;
        _meterSmsFlowService.NotNull(nameof(meterSmsFlowService));
    }

    public async Task CreateAsync(SmsFlowInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await CreateInputValidate(inputDto, cancellationToken);
        SmsFlowInsertDto insertDto = CreateGetInsertDto(inputDto, appUser);
        await CreateExecSql(insertDto);
    }

    private async Task CreateExecSql(SmsFlowInsertDto inputDto)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                SmsFlowCommandService meterSmsFlowCommandService = new(connection, transaction);
                await meterSmsFlowCommandService.Insert(inputDto);
                transaction.Commit();
            }
        }
    }

    private async Task CreateInputValidate(SmsFlowInsertInputDto input, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(input, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    private SmsFlowInsertDto CreateGetInsertDto(SmsFlowInsertInputDto inputDto, IAppUser appUser)
    {
        return new SmsFlowInsertDto()
        {
            FirstFlowId = inputDto.FirstFlowId,
            SmsCount = inputDto.SmsCount,
            SmsTemplateId = inputDto.SmsTemplateId,
            InsertBy = appUser.UserId,
            DueDateTime = ConvertDate.JalaliToDateTime(inputDto.DueDateJalali, true).Value,
        };
    }

    public async Task UpdateAsync(SmsFlowUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        await UpdateExecSql(inputDto);
    }

    private async Task UpdateExecSql(SmsFlowUpdateDto inputDto)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                SmsFlowCommandService meterSmsFlowCommandService = new(connection, transaction);
                await meterSmsFlowCommandService.Update(inputDto);
                transaction.Commit();
            }
        }
    }

    private async Task UpdateInputValidate(SmsFlowUpdateDto input, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(input, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task<SmsFlowGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        SmsFlowGetDto meterSmsFlow = await _meterSmsFlowService.Get(id, true);
        return meterSmsFlow;
    }

    public async Task<IEnumerable<SmsFlowGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<SmsFlowGetDto> meterSmsFlow = await _meterSmsFlowService.Get();
        return meterSmsFlow;
    }
}
