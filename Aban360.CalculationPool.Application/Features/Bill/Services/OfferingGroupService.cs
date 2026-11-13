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

public interface IOfferingGroupService
{
    Task<OfferingGroupGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<OfferingGroupGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(OfferingGroupUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(OfferingGroupDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(OfferingGroupCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class OfferingGroupService : IOfferingGroupService
{
    private readonly IMapper _mapper;
    private readonly IOfferingGroupQueryService _queries;
    private readonly IValidator<OfferingGroupUpdateDto> _updateValidator;
    private readonly IOfferingGroupCommandService _commands;
    private readonly IValidator<OfferingGroupCreateDto> _createValidator;

    public OfferingGroupService(IMapper mapper, IOfferingGroupQueryService queries, IValidator<OfferingGroupUpdateDto> updateValidator, IOfferingGroupCommandService commands, IValidator<OfferingGroupCreateDto> createValidator)
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

    public async Task<OfferingGroupGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        OfferingGroup offeringGroup = await _queries.Get(id);
        return _mapper.Map<OfferingGroupGetDto>(offeringGroup);
    }

    public async Task<ICollection<OfferingGroupGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<OfferingGroup> offeringGroup = await _queries.Get();
        return _mapper.Map<ICollection<OfferingGroupGetDto>>(offeringGroup);
    }

    public async Task UpdateAsync(OfferingGroupUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        OfferingGroup offeringGroup = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, offeringGroup);
    }

    public async Task DeleteAsync(OfferingGroupDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        OfferingGroup offeringGroup = await _queries.Get(deleteDto.Id);
        await _commands.Remove(offeringGroup);
    }

    public async Task CreateAsync(OfferingGroupCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        OfferingGroup offeringGroup = _mapper.Map<OfferingGroup>(createDto);
        await _commands.Add(offeringGroup);
    }
}
