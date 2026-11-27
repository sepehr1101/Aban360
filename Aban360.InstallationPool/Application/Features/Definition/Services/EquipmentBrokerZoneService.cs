using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.InstallationPool.Application.Features.Definition.Services;

public interface IEquipmentBrokerZoneService
{
    Task<EquipmentBrokerZoneGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<EquipmentBrokerZoneGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(EquipmentBrokerZoneUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(EquipmentBrokerZoneDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(EquipmentBrokerZoneCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class EquipmentBrokerZoneService : IEquipmentBrokerZoneService
{
    private readonly IMapper _mapper;
    private readonly IEquipmentBrokerZoneQueryService _equipmentBrokerZoneQueryService;
    private readonly IZoneTitleAddhoc _zoneTitleAddHock;
    private readonly IValidator<EquipmentBrokerZoneUpdateDto> _updateValidator;
    private readonly IEquipmentBrokerZoneCommandService _equipmentBrokerZoneCommandService;
    private readonly IValidator<EquipmentBrokerZoneCreateDto> _createValidator;

    public EquipmentBrokerZoneService(IMapper mapper, IEquipmentBrokerZoneQueryService equipmentBrokerZoneQueryService, IZoneTitleAddhoc zoneTitleAddHock, IValidator<EquipmentBrokerZoneUpdateDto> updateValidator, IEquipmentBrokerZoneCommandService equipmentBrokerZoneCommandService, IValidator<EquipmentBrokerZoneCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        equipmentBrokerZoneQueryService.NotNull(nameof(equipmentBrokerZoneQueryService));
        _equipmentBrokerZoneQueryService = equipmentBrokerZoneQueryService;
        zoneTitleAddHock.NotNull(nameof(zoneTitleAddHock));
        _zoneTitleAddHock = zoneTitleAddHock;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        equipmentBrokerZoneCommandService.NotNull(nameof(equipmentBrokerZoneCommandService));
        _equipmentBrokerZoneCommandService = equipmentBrokerZoneCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<EquipmentBrokerZoneGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var equipmentBrokerZone = await _equipmentBrokerZoneQueryService.Get(id);
        return _mapper.Map<EquipmentBrokerZoneGetDto>(equipmentBrokerZone);
    }

    public async Task<ICollection<EquipmentBrokerZoneGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var equipmentBrokerZone = await _equipmentBrokerZoneQueryService.Get();
        return _mapper.Map<ICollection<EquipmentBrokerZoneGetDto>>(equipmentBrokerZone);
    }

    public async Task UpdateAsync(EquipmentBrokerZoneUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var equipmentBrokerZone = await _equipmentBrokerZoneQueryService.Get(updateDto.Id);
        equipmentBrokerZone.ZoneTitle = await _zoneTitleAddHock.Handle(updateDto.ZoneId, cancellationToken);
        _mapper.Map(updateDto, equipmentBrokerZone);
    }

    public async Task DeleteAsync(EquipmentBrokerZoneDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var equipmentBrokerZone = await _equipmentBrokerZoneQueryService.Get(deleteDto.Id);
        await _equipmentBrokerZoneCommandService.Remove(equipmentBrokerZone);
    }

    public async Task CreateAsync(EquipmentBrokerZoneCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var equipmentBrokerZone = _mapper.Map<EquipmentBrokerZone>(createDto);
        equipmentBrokerZone.ZoneTitle = await _zoneTitleAddHock.Handle(createDto.ZoneId, cancellationToken);
        await _equipmentBrokerZoneCommandService.Add(equipmentBrokerZone);
    }

}
