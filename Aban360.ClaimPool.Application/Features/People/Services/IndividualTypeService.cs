using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.People.Services;

public interface IIndividualTypeService
{
    Task<IndividualTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<IndividualTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(IndividualTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(IndividualTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IndividualTypeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class IndividualTypeService : IIndividualTypeService
{
    private readonly IMapper _mapper;
    private readonly IIndividualTypeQueryService _queries;
    private readonly IValidator<IndividualTypeUpdateDto> _updateValidator;
    private readonly IIndividualTypeCommandService _commands;
    private readonly IValidator<IndividualTypeCreateDto> _createValidator;

    public IndividualTypeService(IMapper mapper, IIndividualTypeQueryService queries, IValidator<IndividualTypeUpdateDto> updateValidator, IIndividualTypeCommandService commands, IValidator<IndividualTypeCreateDto> createValidator)
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

    public async Task<IndividualTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        IndividualType individualType = await _queries.Get(id);
        return _mapper.Map<IndividualTypeGetDto>(individualType);
    }

    public async Task<ICollection<IndividualTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<IndividualType> individualType = await _queries.Get();
        return _mapper.Map<ICollection<IndividualTypeGetDto>>(individualType);
    }

    public async Task UpdateAsync(IndividualTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        IndividualType individualType = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, individualType);
    }

    public async Task DeleteAsync(IndividualTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        IndividualType individualType = await _queries.Get(deleteDto.Id);
        await _commands.Remove(individualType);
    }

    public async Task CreateAsync(IndividualTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        IndividualType individualType = _mapper.Map<IndividualType>(createDto);
        await _commands.Add(individualType);
    }
}
