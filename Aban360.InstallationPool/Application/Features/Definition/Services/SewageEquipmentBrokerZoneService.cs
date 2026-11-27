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

namespace Aban360.InstallationPool.Application.Features.Definition.Services;

public interface ISewageEquipmentBrokerZoneService
{
    Task<SewageEquipmentBrokerZoneGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<SewageEquipmentBrokerZoneGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(SewageEquipmentBrokerZoneUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(SewageEquipmentBrokerZoneDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(SewageEquipmentBrokerZoneCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class SewageEquipmentBrokerZoneService : ISewageEquipmentBrokerZoneService
{
    private readonly IMapper _mapper;
    private readonly ISewageEquipmentBrokerZoneQueryService _sewageEquipmentBrokerZoneQueryService;
    private readonly IZoneTitleAddhoc _zoneTitleAddHock;
    private readonly IValidator<SewageEquipmentBrokerZoneUpdateDto> _updateValidator;
    private readonly ISewageEquipmentBrokerZoneCommandService _sewageEquipmentBrokerZoneCommandService;
    private readonly IValidator<SewageEquipmentBrokerZoneCreateDto> _createValidator;

    public SewageEquipmentBrokerZoneService(IMapper mapper, ISewageEquipmentBrokerZoneQueryService sewageEquipmentBrokerZoneQueryService, IZoneTitleAddhoc zoneTitleAddHock, IValidator<SewageEquipmentBrokerZoneUpdateDto> updateValidator, ISewageEquipmentBrokerZoneCommandService sewageEquipmentBrokerZoneCommandService, IValidator<SewageEquipmentBrokerZoneCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        sewageEquipmentBrokerZoneQueryService.NotNull(nameof(sewageEquipmentBrokerZoneQueryService));
        _sewageEquipmentBrokerZoneQueryService = sewageEquipmentBrokerZoneQueryService;
        zoneTitleAddHock.NotNull(nameof(zoneTitleAddHock));
        _zoneTitleAddHock = zoneTitleAddHock;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        sewageEquipmentBrokerZoneCommandService.NotNull(nameof(sewageEquipmentBrokerZoneCommandService));
        _sewageEquipmentBrokerZoneCommandService = sewageEquipmentBrokerZoneCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<SewageEquipmentBrokerZoneGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var sewageEquipmentBrokerZone = await _sewageEquipmentBrokerZoneQueryService.Get(id);
        return _mapper.Map<SewageEquipmentBrokerZoneGetDto>(sewageEquipmentBrokerZone);
    }

    public async Task<ICollection<SewageEquipmentBrokerZoneGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var sewageEquipmentBrokerZone = await _sewageEquipmentBrokerZoneQueryService.Get();
        return _mapper.Map<ICollection<SewageEquipmentBrokerZoneGetDto>>(sewageEquipmentBrokerZone);
    }

    public async Task UpdateAsync(SewageEquipmentBrokerZoneUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var sewageEquipmentBrokerZone = await _sewageEquipmentBrokerZoneQueryService.Get(updateDto.Id);
        sewageEquipmentBrokerZone.ZoneTitle = await _zoneTitleAddHock.Handle(updateDto.ZoneId, cancellationToken);
        _mapper.Map(updateDto, sewageEquipmentBrokerZone);
    }

    public async Task DeleteAsync(SewageEquipmentBrokerZoneDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var sewageEquipmentBrokerZone = await _sewageEquipmentBrokerZoneQueryService.Get(deleteDto.Id);
        await _sewageEquipmentBrokerZoneCommandService.Remove(sewageEquipmentBrokerZone);
    }

    public async Task CreateAsync(SewageEquipmentBrokerZoneCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var sewageEquipmentBrokerZone = _mapper.Map<SewageEquipmentBrokerZone>(createDto);
        sewageEquipmentBrokerZone.ZoneTitle = await _zoneTitleAddHock.Handle(createDto.ZoneId,cancellationToken);
        await _sewageEquipmentBrokerZoneCommandService.Add(sewageEquipmentBrokerZone);
    }

}
