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

public interface IIndividualTagService
{
    Task<ICollection<IndividualTagGetDto>> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<IndividualTagGetDto> GetBySearchAsync(string input, CancellationToken cancellationToken);
    Task UpdateAsync(IndividualTagUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(IndividualTagDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IndividualTagCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class IndividualTagService : IIndividualTagService
{
    private readonly IMapper _mapper;
    private readonly IIndividualTagQueryService _queries;
    private readonly IValidator<IndividualTagUpdateDto> _updateValidator;
    private readonly IIndividualTagCommandService _commands;
    private readonly IValidator<IndividualTagCreateDto> _createValidator;

    public IndividualTagService(IMapper mapper, IIndividualTagQueryService queries, IValidator<IndividualTagUpdateDto> updateValidator, IIndividualTagCommandService commands, IValidator<IndividualTagCreateDto> createValidator)
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

    public async Task<ICollection<IndividualTagGetDto>> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        IndividualTag individualTag = await _queries.Get(id);
        return _mapper.Map<ICollection<IndividualTagGetDto>>(individualTag);
    }

    public async Task<IndividualTagGetDto> GetBySearchAsync(string input, CancellationToken cancellationToken)
    {
        ICollection<IndividualTag> IndividualTag = await _queries.Get(input);
        return _mapper.Map<IndividualTagGetDto>(IndividualTag);
    }

    public async Task UpdateAsync(IndividualTagUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        IndividualTag individualTag = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, individualTag);
    }

    public async Task DeleteAsync(IndividualTagDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        IndividualTag individualTag = await _queries.Get(deleteDto.Id);
        await _commands.Remove(individualTag);
    }

    public async Task CreateAsync(IndividualTagCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        IndividualTag individualTag = _mapper.Map<IndividualTag>(createDto);
        individualTag.InsertLogInfo = " ";//todo
        individualTag.ValidFrom=DateTime.Now;
        await _commands.Add(individualTag);
    }
}
