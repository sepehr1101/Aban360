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

public interface IOfferingService
{
    Task<OfferingGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<OfferingGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(OfferingUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(OfferingDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(OfferingCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class OfferingService : IOfferingService
{
    private readonly IMapper _mapper;
    private readonly IOfferingQueryService _queries;
    private readonly IValidator<OfferingUpdateDto> _updateValidator;
    private readonly IOfferingCommandService _commands;
    private readonly IValidator<OfferingCreateDto> _createValidator;

    public OfferingService(IMapper mapper, IOfferingQueryService queries, IValidator<OfferingUpdateDto> updateValidator, IOfferingCommandService commands, IValidator<OfferingCreateDto> createValidator)
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

    public async Task<OfferingGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        Offering offering = await _queries.Get(id);
        return _mapper.Map<OfferingGetDto>(offering);
    }

    public async Task<ICollection<OfferingGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Offering> offering = await _queries.Get();
        return _mapper.Map<ICollection<OfferingGetDto>>(offering);
    }

    public async Task UpdateAsync(OfferingUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
        Offering offering = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, offering);
    }

    public async Task DeleteAsync(OfferingDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Offering offering = await _queries.Get(deleteDto.Id);
        await _commands.Remove(offering);
    }

    public async Task CreateAsync(OfferingCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Offering offering = _mapper.Map<Offering>(createDto);
        await _commands.Add(offering);
    }
}
