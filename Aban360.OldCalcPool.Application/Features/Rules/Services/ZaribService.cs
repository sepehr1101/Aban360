using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
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

public interface IZaribService
{
    Task CreateAsync(ZaribCreateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task UpdateAsync(ZaribUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task<ZaribGetDto> GetAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<ZaribGetDto>> GetAllAsync(CancellationToken cancellationToken);
}

internal sealed class ZaribService : AbstractBaseConnection, IZaribService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IT51QueryService _t51QueryService;
    private readonly IValidator<ZaribCreateInputDto> _createValidator;
    private readonly IZaribGetService _zaribGetService;
    private readonly IValidator<ZaribUpdateInputDto> _updateValidator;
    private readonly IZaribGetAllService _zaribGetAllService;
    public ZaribService(
        IHttpContextAccessor contextAccessor,
        IT51QueryService t51QueryService,
        IValidator<ZaribCreateInputDto> validator,
        IConfiguration configuration,
        IZaribGetService zaribGetService,
        IValidator<ZaribUpdateInputDto> updateValidator,
        IZaribGetAllService zaribGetAllService) : base(configuration)
    {
        _contextAccessor = contextAccessor;
        _contextAccessor.NotNull(nameof(contextAccessor));
        _t51QueryService = t51QueryService;
        _t51QueryService.NotNull(nameof(t51QueryService));
        _createValidator = validator;
        _createValidator.NotNull(nameof(_createValidator));
        _zaribGetService = zaribGetService;
        _zaribGetService.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        _updateValidator.NotNull(nameof(_updateValidator));
        _zaribGetService.NotNull(nameof(zaribGetService));
        _zaribGetAllService = zaribGetAllService;
        _zaribGetAllService.NotNull(nameof(zaribGetAllService));
    }

    public async Task CreateAsync(ZaribCreateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await CreateInputValidate(inputDto, cancellationToken);
        ZaribCreateDto createDto = await CreateGetCreateDto(inputDto);
        string opLogText = string.Format(OpLogLiterals.ZaribInsertOpLog, createDto.ZoneTitle1, createDto.Zarib_baha);
        await CreateExecSql(createDto, appUser, opLogText);
    }

    private async Task CreateExecSql(ZaribCreateDto createDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                ZaribCommandService zaribCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await zaribCommandService.Insert(createDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task<ZaribCreateDto> CreateGetCreateDto(ZaribCreateInputDto inputDto)
    {
        NumericDictionary zoneInfo = await _t51QueryService.Get(inputDto.ZoneId1, true);
        return new ZaribCreateDto()
        {
            Town = inputDto.Town,
            ZoneTitle1 = zoneInfo.Title,
            ZoneTitle2 = zoneInfo.Title,
            Zarib_baha = inputDto.Zarib_baha,
            Date1 = inputDto.Date1,
            Date2 = inputDto.Date2,
            Zb = inputDto.Zb,
            Zb1 = inputDto.Zb1,
            Zb2 = inputDto.Zb2,
            Zb3 = inputDto.Zb3,
            Zb4 = inputDto.Zb4,
            Zb5 = inputDto.Zb5,
            Zb6 = inputDto.Zb6,
            Zb7 = inputDto.Zb7,
            Zb8 = inputDto.Zb8,
            Zb_r = inputDto.Zb_r,
        };
    }

    private async Task CreateInputValidate(ZaribCreateInputDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task UpdateAsync(ZaribUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await UpdateInputValidate(inputDto, cancellationToken);
        ZaribGetDto zaribInfo = await _zaribGetService.Get(inputDto.Id);
        ZaribUpdateDto updateDto = await UpdateGetUpdateDto(inputDto);
        string opLogText = string.Format(OpLogLiterals.ZaribUpdateOpLog, zaribInfo.ZoneTitle1, updateDto.ZoneTitle1, zaribInfo.Zarib_baha, updateDto.Zarib_baha);
        await UpdateExecSql(updateDto, appUser, opLogText);
    }

    private async Task UpdateExecSql(ZaribUpdateDto createDto, IAppUser appUser, string opLogText)
    {
        using (IDbConnection connection = _sqlReportConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                ZaribCommandService zaribCommandService = new(connection, transaction);
                OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                await zaribCommandService.Update(createDto);
                await opLogCommandService.Insert(opLogText, appUser);
                transaction.Commit();
            }
        }
    }

    private async Task<ZaribUpdateDto> UpdateGetUpdateDto(ZaribUpdateInputDto inputDto)
    {
        NumericDictionary zoneInfo = await _t51QueryService.Get(inputDto.ZoneId1, true);
        return new ZaribUpdateDto()
        {
            Id = inputDto.Id,
            Town = inputDto.Town,
            ZoneTitle1 = zoneInfo.Title,
            ZoneTitle2 = zoneInfo.Title,
            Zarib_baha = inputDto.Zarib_baha,
            Date1 = inputDto.Date1,
            Date2 = inputDto.Date2,
            Zb = inputDto.Zb,
            Zb1 = inputDto.Zb1,
            Zb2 = inputDto.Zb2,
            Zb3 = inputDto.Zb3,
            Zb4 = inputDto.Zb4,
            Zb5 = inputDto.Zb5,
            Zb6 = inputDto.Zb6,
            Zb7 = inputDto.Zb7,
            Zb8 = inputDto.Zb8,
            Zb_r = inputDto.Zb_r,
        };
    }

    private async Task UpdateInputValidate(ZaribUpdateInputDto UpdateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(UpdateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }

    public async Task<ZaribGetDto> GetAsync(int id, CancellationToken cancellationToken)
    {
        ZaribGetDto result = await _zaribGetService.Get(id);
        return result;
    }

    public async Task<IEnumerable<ZaribGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<ZaribGetDto> result = await _zaribGetAllService.Get();
        return result;
    }
}
