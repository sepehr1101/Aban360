using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Metering.Services;

public interface IWaterMeterInstallationMethodService
{
    Task<WaterMeterInstallationMethodGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<WaterMeterInstallationMethodGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(WaterMeterInstallationMethodUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(WaterMeterInstallationMethodDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(WaterMeterInstallationMethodCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class WaterMeterInstallationMethodService : IWaterMeterInstallationMethodService
{
    private readonly IMapper _mapper;
    private readonly IWaterMeterInstallationMethodQueryService _queries;
    private readonly IValidator<WaterMeterInstallationMethodUpdateDto> _updateValidator;
    private readonly IWaterMeterInstallationMethodCommandService _commands;
    private readonly IValidator<WaterMeterInstallationMethodCreateDto> _createValidator;

    public WaterMeterInstallationMethodService(IMapper mapper, IWaterMeterInstallationMethodQueryService queries, IValidator<WaterMeterInstallationMethodUpdateDto> updateValidator, IWaterMeterInstallationMethodCommandService commands, IValidator<WaterMeterInstallationMethodCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queries.NotNull(nameof(queries));
        _queries = queries;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        commands.NotNull(nameof(commands));
        _commands = commands;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<WaterMeterInstallationMethodGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var waterMeterInstallationMethod = await _queries.Get(id);
        return _mapper.Map<WaterMeterInstallationMethodGetDto>(waterMeterInstallationMethod);
    }

    public async Task<ICollection<WaterMeterInstallationMethodGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var waterMeterInstallationMethod = await _queries.Get();
        return _mapper.Map<ICollection<WaterMeterInstallationMethodGetDto>>(waterMeterInstallationMethod);
    }

    public async Task UpdateAsync(WaterMeterInstallationMethodUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var waterMeterInstallationMethod = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, waterMeterInstallationMethod);
    }

    public async Task DeleteAsync(WaterMeterInstallationMethodDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var waterMeterInstallationMethod = await _queries.Get(deleteDto.Id);
        await _commands.Remove(waterMeterInstallationMethod);
    }

    public async Task CreateAsync(WaterMeterInstallationMethodCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var waterMeterInstallationMethod = _mapper.Map<WaterMeterInstallationMethod>(createDto);
        await _commands.Add(waterMeterInstallationMethod);
    }
}
