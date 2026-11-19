using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Metering.Services;
public interface IWaterMeterService
{
    Task CreateAsync(WaterMeterCreateDto createDto, CancellationToken cancellationToken);
    Task UpdateAsync(WaterMeterUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(WaterMeterDeleteDto deleteDto, CancellationToken cancellationToken);
    Task<ICollection<WaterMeterGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<WaterMeterGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
}

internal sealed class WaterMeterService : IWaterMeterService
{
    private readonly IMapper _mapper;
    private readonly IWaterMeterCommandService _commandService;
    private readonly IValidator<WaterMeterCreateDto> _waterMeterValidator;
    private readonly IWaterMeterQueryService _queryService;
    private readonly IValidator<WaterMeterUpdateDto> _updateValidator;
    public WaterMeterService(IMapper mapper, IWaterMeterCommandService commandService, IValidator<WaterMeterCreateDto> waterMeterValidator, IWaterMeterQueryService queryService, IValidator<WaterMeterUpdateDto> waterMeterVlidator)
    {
        _mapper = mapper;
        _mapper.NotNull(nameof(mapper));
        _commandService = commandService;
        _commandService.NotNull(nameof(commandService));
        _waterMeterValidator = waterMeterValidator;
        _waterMeterValidator.NotNull(nameof(waterMeterValidator));
        _queryService = queryService;
        _queryService.NotNull(nameof(queryService));
        _updateValidator = waterMeterVlidator;
        _updateValidator.NotNull(nameof(waterMeterVlidator));
    }

    public async Task CreateAsync(WaterMeterCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _waterMeterValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            string message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }

        WaterMeter waterMeter = _mapper.Map<WaterMeter>(createDto);
        waterMeter.ValidFrom = DateTime.Now;
        waterMeter.InsertLogInfo = "SampleLogInfo";
        waterMeter.Hash = "SampleHash";
        await _commandService.Add(waterMeter);
    }

    public async Task UpdateAsync(WaterMeterUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            string message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }

        WaterMeter waterMeter = await _queryService.Get(updateDto.Id);
        waterMeter.ValidFrom = DateTime.Now;
        waterMeter.InsertLogInfo = "SampleLogInfo";
        waterMeter.Hash = "SampleHash";
        _mapper.Map(updateDto, waterMeter);
    }

    public async Task DeleteAsync(WaterMeterDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        WaterMeter waterMeter = await _queryService.Get(deleteDto.Id);
        if (waterMeter == null)
        {
            throw new InvalidDataException();
        }

        await _commandService.Remove(waterMeter);
    }

    public async Task<ICollection<WaterMeterGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<WaterMeter> waterMeter = await _queryService.Get();
        if (waterMeter == null)
        {
            throw new InvalidDataException();
        }

        return _mapper.Map<ICollection<WaterMeterGetDto>>(waterMeter);
    }

    public async Task<WaterMeterGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        WaterMeter waterMeter = await _queryService.Get(id);
        if (waterMeter == null)
        {
            throw new InvalidDataException();
        }

        return _mapper.Map<WaterMeterGetDto>(waterMeter);
    }
}
