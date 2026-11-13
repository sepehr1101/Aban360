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

namespace Aban360.ClaimPool.Application.Features.WasteWater.Services;

public interface IWaterMeterTagDefinitionService
{
    Task<WaterMeterTagDefinitionGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<WaterMeterTagDefinitionGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(WaterMeterTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(WaterMeterTagDefinitionDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(WaterMeterTagDefinitionCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class WaterMeterTagDefinitionService : IWaterMeterTagDefinitionService
{
    private readonly IMapper _mapper;
    private readonly IWaterMeterTagDefinitionQueryService _queryService;
    private readonly IValidator<WaterMeterTagDefinitionUpdateDto> _updateValidator;
    private readonly IWaterMeterTagDefinitionCommandService _commandService;
    private readonly IValidator<WaterMeterTagDefinitionCreateDto> _createValidator;

    public WaterMeterTagDefinitionService(IMapper mapper, IWaterMeterTagDefinitionQueryService queryService, IValidator<WaterMeterTagDefinitionUpdateDto> updateValidator, IWaterMeterTagDefinitionCommandService commandService, IValidator<WaterMeterTagDefinitionCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queryService.NotNull(nameof(queryService));
        _queryService = queryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        commandService.NotNull(nameof(commandService));
        _commandService = commandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<WaterMeterTagDefinitionGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        WaterMeterTagDefinition waterMeterTagDefinition = await _queryService.Get(id);
        return _mapper.Map<WaterMeterTagDefinitionGetDto>(waterMeterTagDefinition);
    }

    public async Task<ICollection<WaterMeterTagDefinitionGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<WaterMeterTagDefinition> waterMeterTagDefinition = await _queryService.Get();
        return _mapper.Map<ICollection<WaterMeterTagDefinitionGetDto>>(waterMeterTagDefinition);
    }

    public async Task UpdateAsync(WaterMeterTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        WaterMeterTagDefinition waterMeterTagDefinition = await _queryService.Get(updateDto.Id);
        _mapper.Map(updateDto, waterMeterTagDefinition);
    }

    public async Task DeleteAsync(WaterMeterTagDefinitionDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        WaterMeterTagDefinition waterMeterTagDefinition = await _queryService.Get(deleteDto.Id);
        await _commandService.Remove(waterMeterTagDefinition);
    }

    public async Task CreateAsync(WaterMeterTagDefinitionCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var waterMeterTagDefinition = _mapper.Map<WaterMeterTagDefinition>(createDto);
        await _commandService.Add(waterMeterTagDefinition);
    }

}
