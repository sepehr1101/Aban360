using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.InstallationPool.Application.Features.Definition.Services;

public interface ISewageEquipmentBrokerService
{
    Task<SewageEquipmentBrokerGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<SewageEquipmentBrokerGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(SewageEquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(SewageEquipmentBrokerDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(SewageEquipmentBrokerCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class SewageEquipmentBrokerService : ISewageEquipmentBrokerService
{
    private readonly IMapper _mapper;
    private readonly ISewageEquipmentBrokerQueryService _queries;
    private readonly IValidator<SewageEquipmentBrokerUpdateDto> _updateValidator;
    private readonly ISewageEquipmentBrokerCommandService _commands;
    private readonly IValidator<SewageEquipmentBrokerCreateDto> _createValidator;

    public SewageEquipmentBrokerService(IMapper mapper, ISewageEquipmentBrokerQueryService queries, IValidator<SewageEquipmentBrokerUpdateDto> updateValidator, ISewageEquipmentBrokerCommandService commands, IValidator<SewageEquipmentBrokerCreateDto> createValidator)
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

    public async Task<SewageEquipmentBrokerGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var sewageEquipmentBroker = await _queries.Get(id);
        return _mapper.Map<SewageEquipmentBrokerGetDto>(sewageEquipmentBroker);
    }

    public async Task<ICollection<SewageEquipmentBrokerGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var sewageEquipmentBroker = await _queries.Get();
        return _mapper.Map<ICollection<SewageEquipmentBrokerGetDto>>(sewageEquipmentBroker);
    }

    public async Task UpdateAsync(SewageEquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var sewageEquipmentBroker = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, sewageEquipmentBroker);
    }

    public async Task DeleteAsync(SewageEquipmentBrokerDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var sewageEquipmentBroker = await _queries.Get(deleteDto.Id);
        await _commands.Remove(sewageEquipmentBroker);
    }

    public async Task CreateAsync(SewageEquipmentBrokerCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var sewageEquipmentBroker = _mapper.Map<SewageEquipmentBroker>(createDto);
        await _commands.Add(sewageEquipmentBroker);
    }
}
