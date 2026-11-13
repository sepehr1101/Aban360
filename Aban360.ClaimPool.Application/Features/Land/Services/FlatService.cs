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

public interface IFlatService
{
    Task<FlatGetDto> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<FlatGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(FlatUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(FlatDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(FlatCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class FlatService : IFlatService
{
    private readonly IMapper _mapper;
    private readonly IFlatQueryService _queryService;
    private readonly IValidator<FlatUpdateDto> _updateValidator;
    private readonly IFlatCommandService _commandService;
    private readonly IValidator<FlatCreateDto> _createValidator;

    public FlatService(IMapper mapper, IFlatQueryService queryService, IValidator<FlatUpdateDto> updateValidator, IFlatCommandService commandService, IValidator<FlatCreateDto> createValidator)
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

    public async Task<FlatGetDto> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        Flat flat = await _queryService.Get(id);
        if (flat == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<FlatGetDto>(flat);
    }

    public async Task<ICollection<FlatGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<Flat> flat = await _queryService.Get();
        if (flat == null)
        {
            throw new InvalidDataException();
        }
        return _mapper.Map<ICollection<FlatGetDto>>(flat);
    }

    public async Task UpdateAsync(FlatUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    
    
        Flat flat = await _queryService.Get(updateDto.Id);
        _mapper.Map(updateDto, flat);
    }

    public async Task DeleteAsync(FlatDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        Flat flat = await _queryService.Get(deleteDto.Id);
        await _commandService.Remove(flat);
    
    }

    public async Task CreateAsync(FlatCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    
        Flat flat = _mapper.Map<Flat>(createDto);
        await _commandService.Add(flat);
    }

}
