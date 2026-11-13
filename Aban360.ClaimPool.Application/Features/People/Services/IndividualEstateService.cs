using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.People.Services;

public interface IIndividualEstateService
{
    Task<IndividualEstateGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<IndividualEstateGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(IndividualEstateUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(IndividualEstateDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IndividualEstateCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class IndividualEstateService : IIndividualEstateService
{
    private readonly IMapper _mapper;
    private readonly IIndividualEstateQueryService _queries;
    private readonly IValidator<IndividualEstateUpdateDto> _updateValidator;
    private readonly IIndividualEstateCommandService _commands;
    private readonly IValidator<IndividualEstateCreateDto> _createValidator;

    public IndividualEstateService(IMapper mapper, IIndividualEstateQueryService queries, IValidator<IndividualEstateUpdateDto> updateValidator, IIndividualEstateCommandService commands, IValidator<IndividualEstateCreateDto> createValidator)
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

    public async Task<IndividualEstateGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        IndividualEstate individualEstate = await _queries.Get(id);
        return _mapper.Map<IndividualEstateGetDto>(individualEstate);
    }

    public async Task<ICollection<IndividualEstateGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<IndividualEstate> individualEstate = await _queries.Get();
        return _mapper.Map<ICollection<IndividualEstateGetDto>>(individualEstate);
    }

    public async Task UpdateAsync(IndividualEstateUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        IndividualEstate individualEstate = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, individualEstate);
    }

    public async Task DeleteAsync(IndividualEstateDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        IndividualEstate individualEstate = await _queries.Get(deleteDto.Id);
        await _commands.Remove(individualEstate);
    }

    public async Task CreateAsync(IndividualEstateCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        IndividualEstate individualEstate = _mapper.Map<IndividualEstate>(createDto);
        await _commands.Add(individualEstate);
    }
}
