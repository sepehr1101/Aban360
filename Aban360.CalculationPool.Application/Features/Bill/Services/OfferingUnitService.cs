using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Services;

public interface IOfferingUnitService
{
    Task<OfferingUnitGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<OfferingUnitGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(OfferingUnitUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(OfferingUnitDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(OfferingUnitCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class OfferingUnitService : IOfferingUnitService
{
    private readonly IMapper _mapper;
    private readonly IOfferingUnitQueryService _queries;
    private readonly IValidator<OfferingUnitUpdateDto> _updateValidator;
    private readonly IOfferingUnitCommandService _commands;
    private readonly IValidator<OfferingUnitCreateDto> _createValidator;

    public OfferingUnitService(IMapper mapper, IOfferingUnitQueryService queries, IValidator<OfferingUnitUpdateDto> updateValidator, IOfferingUnitCommandService commands, IValidator<OfferingUnitCreateDto> createValidator)
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

    public async Task<OfferingUnitGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        OfferingUnit offeringUnit = await _queries.Get(id);
        return _mapper.Map<OfferingUnitGetDto>(offeringUnit);
    }

    public async Task<ICollection<OfferingUnitGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<OfferingUnit> offeringUnit = await _queries.Get();
        return _mapper.Map<ICollection<OfferingUnitGetDto>>(offeringUnit);
    }

    public async Task UpdateAsync(OfferingUnitUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        OfferingUnit offeringUnit = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, offeringUnit);
    }

    public async Task DeleteAsync(OfferingUnitDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        OfferingUnit offeringUnit = await _queries.Get(deleteDto.Id);
        await _commands.Remove(offeringUnit);
    }

    public async Task CreateAsync(OfferingUnitCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        OfferingUnit offeringUnit = _mapper.Map<OfferingUnit>(createDto);
        await _commands.Add(offeringUnit);
    }
}
