using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.InstallationPool.Application.Features.Definition.Services;

public interface IEquipmentBrokerService
{
    Task<EquipmentBrokerGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<EquipmentBrokerGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(EquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(EquipmentBrokerDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(EquipmentBrokerCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class EquipmentBrokerService : IEquipmentBrokerService
{
    private readonly IMapper _mapper;
    private readonly IEquipmentBrokerQueryService _queries;
    private readonly IValidator<EquipmentBrokerUpdateDto> _updateValidator;
    private readonly IEquipmentBrokerCommandService _commands;
    private readonly IValidator<EquipmentBrokerCreateDto> _createValidator;

    public EquipmentBrokerService(IMapper mapper, IEquipmentBrokerQueryService queries, IValidator<EquipmentBrokerUpdateDto> updateValidator, IEquipmentBrokerCommandService commands, IValidator<EquipmentBrokerCreateDto> createValidator)
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

    public async Task<EquipmentBrokerGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var equipmentBroker = await _queries.Get(id);
        return _mapper.Map<EquipmentBrokerGetDto>(equipmentBroker);
    }

    public async Task<ICollection<EquipmentBrokerGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var equipmentBroker = await _queries.Get();
        return _mapper.Map<ICollection<EquipmentBrokerGetDto>>(equipmentBroker);
    }

    public async Task UpdateAsync(EquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var equipmentBroker = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, equipmentBroker);
    }

    public async Task DeleteAsync(EquipmentBrokerDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var equipmentBroker = await _queries.Get(deleteDto.Id);
        await _commands.Remove(equipmentBroker);
    }

    public async Task CreateAsync(EquipmentBrokerCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var equipmentBroker = _mapper.Map<EquipmentBroker>(createDto);
        await _commands.Add(equipmentBroker);
    }
}
