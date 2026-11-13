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

public interface IConstructionTypeService
{
    Task<ConstructionTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<ConstructionTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(ConstructionTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(ConstructionTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(ConstructionTypeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class ConstructionTypeService : IConstructionTypeService
{
    private readonly IMapper _mapper;
    private readonly IConstructionTypeQueryService _queryService;
    private readonly IValidator<ConstructionTypeUpdateDto> _updateValidator;
    private readonly IConstructionTypeCommandService _commandService;
    private readonly IValidator<ConstructionTypeCreateDto> _createValidator;

    public ConstructionTypeService(IMapper mapper, IConstructionTypeQueryService queryService, IValidator<ConstructionTypeUpdateDto> updateValidator, IConstructionTypeCommandService commandService, IValidator<ConstructionTypeCreateDto> createValidator)
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

    public async Task<ConstructionTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        ConstructionType constructionType = await _queryService.Get(id);
        if (constructionType == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ConstructionTypeGetDto>(constructionType);
    }

    public async Task<ICollection<ConstructionTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<ConstructionType> constructionType = await _queryService.Get();
        if (constructionType == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<ConstructionTypeGetDto>>(constructionType);
    }

    public async Task UpdateAsync(ConstructionTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        ConstructionType constructionType = await _queryService.Get(updateDto.Id);
        _mapper.Map(updateDto, constructionType);
    }

    public async Task DeleteAsync(ConstructionTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        ConstructionType constructionType = await _queryService.Get(deleteDto.Id);
        await _commandService.Remove(constructionType);
    }

    public async Task CreateAsync(ConstructionTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        ConstructionType constructionType = _mapper.Map<ConstructionType>(createDto);
        await _commandService.Add(constructionType);
    }

}
