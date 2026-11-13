using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Services;

public interface IWaterMeterTagService
{
    Task<ICollection<WaterMeterTagGetDto>> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<WaterMeterTagGetDto> GetBySearchAsync(string input, CancellationToken cancellationToken);
    Task UpdateAsync(WaterMeterTagUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(WaterMeterTagDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(WaterMeterTagCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class WaterMeterTagService : IWaterMeterTagService
{
    private readonly IMapper _mapper;
    private readonly IWaterMeterTagQueryService _queries;
    private readonly IValidator<WaterMeterTagUpdateDto> _updateValidator;
    private readonly IWaterMeterTagCommandService _commands;
    private readonly IValidator<WaterMeterTagCreateDto> _createValidator;

    public WaterMeterTagService(IMapper mapper, IWaterMeterTagQueryService queries, IValidator<WaterMeterTagUpdateDto> updateValidator, IWaterMeterTagCommandService commands, IValidator<WaterMeterTagCreateDto> createValidator)
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

    public async Task<ICollection<WaterMeterTagGetDto>> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        WaterMeterTag waterMeterTag = await _queries.Get(id);
        return _mapper.Map<ICollection<WaterMeterTagGetDto>>(waterMeterTag);
    }

    public async Task<WaterMeterTagGetDto> GetBySearchAsync(string input, CancellationToken cancellationToken)
    {
        ICollection<WaterMeterTag> waterMeterTag = await _queries.Get(input);
        return _mapper.Map<WaterMeterTagGetDto>(waterMeterTag);
    }

    public async Task UpdateAsync(WaterMeterTagUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var waterMeterTag = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, waterMeterTag);
    }

    public async Task DeleteAsync(WaterMeterTagDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var waterMeterTag = await _queries.Get(deleteDto.Id);
        await _commands.Remove(waterMeterTag);
    }

    public async Task CreateAsync(WaterMeterTagCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        WaterMeterTag waterMeterTag = _mapper.Map<WaterMeterTag>(createDto);
        await _commands.Add(waterMeterTag);
    }
}
