using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Land.Services;

public interface IEstateBoundTypeService
{
    Task<EstateBoundTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<EstateBoundTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(EstateBoundTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(EstateBoundTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(EstateBoundTypeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class EstateBoundTypeService : IEstateBoundTypeService
{
    private readonly IMapper _mapper;
    private readonly IEstateBoundTypeQueryService _queryService;
    private readonly IValidator<EstateBoundTypeUpdateDto> _updateValidator;
    private readonly IEstateBoundTypeCommandService _commandService;
    private readonly IValidator<EstateBoundTypeCreateDto> _createValidator;

    public EstateBoundTypeService(IMapper mapper, IEstateBoundTypeQueryService queryService, IValidator<EstateBoundTypeUpdateDto> updateValidator, IEstateBoundTypeCommandService commandService, IValidator<EstateBoundTypeCreateDto> createValidator)
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

    public async Task<EstateBoundTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        EstateBoundType estateBoundType = await _queryService.Get(id);
        if (estateBoundType == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<EstateBoundTypeGetDto>(estateBoundType);
    }

    public async Task<ICollection<EstateBoundTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<EstateBoundType> estateBoundType = await _queryService.Get();
        if (estateBoundType == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<EstateBoundTypeGetDto>>(estateBoundType);
    }

    public async Task UpdateAsync(EstateBoundTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        EstateBoundType estateBoundType = await _queryService.Get(updateDto.Id);
        _mapper.Map(updateDto, estateBoundType);
    }

    public async Task DeleteAsync(EstateBoundTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        EstateBoundType estateBoundType = await _queryService.Get(deleteDto.Id);
        await _commandService.Remove(estateBoundType);
    }

    public async Task CreateAsync(EstateBoundTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        EstateBoundType estateBoundType = _mapper.Map<EstateBoundType>(createDto);
        await _commandService.Add(estateBoundType);
    }

}
