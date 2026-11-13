using Aban360.ClaimPool.Domain.Constants;
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

public interface IIndividualEstateRelationTypeService
{
    Task<IndividualEstateRelationTypeGetDto> GetSingleAsync(IndividualEstateRelationTypeEnum id, CancellationToken cancellationToken);
    Task<ICollection<IndividualEstateRelationTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(IndividualEstateRelationTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(IndividualEstateRelationTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IndividualEstateRelationTypeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class IndividualEstateRelationTypeService : IIndividualEstateRelationTypeService
{
    private readonly IMapper _mapper;
    private readonly IIndividualEstateRelationTypeQueryService _queries;
    private readonly IValidator<IndividualEstateRelationTypeUpdateDto> _updateValidator;
    private readonly IIndividualEstateRelationTypeCommandService _commands;
    private readonly IValidator<IndividualEstateRelationTypeCreateDto> _createValidator;

    public IndividualEstateRelationTypeService(IMapper mapper, IIndividualEstateRelationTypeQueryService queries, IValidator<IndividualEstateRelationTypeUpdateDto> updateValidator, IIndividualEstateRelationTypeCommandService commands, IValidator<IndividualEstateRelationTypeCreateDto> createValidator)
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

    public async Task<IndividualEstateRelationTypeGetDto> GetSingleAsync(IndividualEstateRelationTypeEnum id, CancellationToken cancellationToken)
    {
        IndividualEstateRelationType individualEstateRelationType = await _queries.Get(id);
        return _mapper.Map<IndividualEstateRelationTypeGetDto>(individualEstateRelationType);
    }

    public async Task<ICollection<IndividualEstateRelationTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<IndividualEstateRelationType> individualEstateRelationType = await _queries.Get();
        return _mapper.Map<ICollection<IndividualEstateRelationTypeGetDto>>(individualEstateRelationType);
    }

    public async Task UpdateAsync(IndividualEstateRelationTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        IndividualEstateRelationType individualEstateRelationType = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, individualEstateRelationType);
    }

    public async Task DeleteAsync(IndividualEstateRelationTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        IndividualEstateRelationType individualEstateRelationType = await _queries.Get(deleteDto.Id);
        await _commands.Remove(individualEstateRelationType);
    }

    public async Task CreateAsync(IndividualEstateRelationTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        IndividualEstateRelationType individualEstateRelationType = _mapper.Map<IndividualEstateRelationType>(createDto);
        await _commands.Add(individualEstateRelationType);
    }
}
