using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Services;

public interface ICapacityCalculationIndexService
{
    Task<CapacityCalculationIndexGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<CapacityCalculationIndexGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(CapacityCalculationIndexUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(CapacityCalculationIndexDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(CapacityCalculationIndexCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class CapacityCalculationIndexService : ICapacityCalculationIndexService
{
    private readonly IMapper _mapper;
    private readonly ICapacityCalculationIndexQueryService _queries;
    private readonly IValidator<CapacityCalculationIndexUpdateDto> _updateValidator;
    private readonly ICapacityCalculationIndexCommandService _commands;
    private readonly IValidator<CapacityCalculationIndexCreateDto> _createValidator;

    public CapacityCalculationIndexService(IMapper mapper, ICapacityCalculationIndexQueryService queries, IValidator<CapacityCalculationIndexUpdateDto> updateValidator, ICapacityCalculationIndexCommandService commands, IValidator<CapacityCalculationIndexCreateDto> createValidator)
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

    public async Task<CapacityCalculationIndexGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var capacityCalculationIndex = await _queries.Get(id);
        return _mapper.Map<CapacityCalculationIndexGetDto>(capacityCalculationIndex);
    }

    public async Task<ICollection<CapacityCalculationIndexGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var capacityCalculationIndex = await _queries.Get();
        return _mapper.Map<ICollection<CapacityCalculationIndexGetDto>>(capacityCalculationIndex);
    }

    public async Task UpdateAsync(CapacityCalculationIndexUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }


        var capacityCalculationIndex = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, capacityCalculationIndex);
    }

    public async Task DeleteAsync(CapacityCalculationIndexDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var capacityCalculationIndex = await _queries.Get(deleteDto.Id);
        await _commands.Remove(capacityCalculationIndex);
    }

    public async Task CreateAsync(CapacityCalculationIndexCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var capacityCalculationIndex = _mapper.Map<CapacityCalculationIndex>(createDto);
        await _commands.Add(capacityCalculationIndex);
    }
}
