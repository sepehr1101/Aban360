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

public interface IIndividualTagDefinitionService
{
    Task<IndividualTagDefinitionGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<IndividualTagDefinitionGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(IndividualTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(IndividualTagDefinitionDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(IndividualTagDefinitionCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class IndividualTagDefinitionService : IIndividualTagDefinitionService
{
    private readonly IMapper _mapper;
    private readonly IIndividualTagDefinitionQueryService _queryService;
    private readonly IValidator<IndividualTagDefinitionUpdateDto> _updateValidator;
    private readonly IIndividualTagDefinitionCommandService _commandService;
    private readonly IValidator<IndividualTagDefinitionCreateDto> _createValidator;

    public IndividualTagDefinitionService(IMapper mapper, IIndividualTagDefinitionQueryService queryService, IValidator<IndividualTagDefinitionUpdateDto> updateValidator, IIndividualTagDefinitionCommandService commandService, IValidator<IndividualTagDefinitionCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        queryService.NotNull(nameof(queryService));
        _queryService = queryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        commandService.NotNull(nameof(commandService));
        _commandService = commandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<IndividualTagDefinitionGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        IndividualTagDefinition individualTagDefinition = await _queryService.Get(id);
        return _mapper.Map<IndividualTagDefinitionGetDto>(individualTagDefinition);
    }

    public async Task<ICollection<IndividualTagDefinitionGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<IndividualTagDefinition> individualTagDefinition = await _queryService.Get();
        return _mapper.Map<ICollection<IndividualTagDefinitionGetDto>>(individualTagDefinition);
    }

    public async Task UpdateAsync(IndividualTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        IndividualTagDefinition individualTagDefinition = await _queryService.Get(updateDto.Id);
        _mapper.Map(updateDto, individualTagDefinition);
    }

    public async Task DeleteAsync(IndividualTagDefinitionDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        IndividualTagDefinition individualTagDefinition = await _queryService.Get(deleteDto.Id);
        await _commandService.Remove(individualTagDefinition);
    }

    public async Task CreateAsync(IndividualTagDefinitionCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        IndividualTagDefinition individualTagDefinition = _mapper.Map<IndividualTagDefinition>(createDto);
        await _commandService.Add(individualTagDefinition);
    }

}
