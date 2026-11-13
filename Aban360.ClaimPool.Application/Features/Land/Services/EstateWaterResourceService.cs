using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Land.Services;

public interface IEstateWaterResourceService
{
    Task<EstateWaterResourceGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<EstateWaterResourceGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(EstateWaterResourceUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(EstateWaterResourceDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(EstateWaterResourceCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class EstateWaterResourceService : IEstateWaterResourceService
{
    private readonly IMapper _mapper;
    private readonly IEstateWaterResourceQueryService _queries;
    private readonly IValidator<EstateWaterResourceUpdateDto> _updateValidator;
    private readonly IEstateWaterResourceCommandService _commands;
    private readonly IValidator<EstateWaterResourceCreateDto> _createValidator;

    public EstateWaterResourceService(IMapper mapper, IEstateWaterResourceQueryService queries, IValidator<EstateWaterResourceUpdateDto> updateValidator, IEstateWaterResourceCommandService commands, IValidator<EstateWaterResourceCreateDto> createValidator)
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

    public async Task<EstateWaterResourceGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        EstateWaterResource estateWaterResource = await _queries.Get(id);
        return _mapper.Map<EstateWaterResourceGetDto>(estateWaterResource);
    }

    public async Task<ICollection<EstateWaterResourceGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<EstateWaterResource> estateWaterResource = await _queries.Get();
        return _mapper.Map<ICollection<EstateWaterResourceGetDto>>(estateWaterResource);
    }

    public async Task UpdateAsync(EstateWaterResourceUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        EstateWaterResource estateWaterResource = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, estateWaterResource);
    }

    public async Task DeleteAsync(EstateWaterResourceDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        EstateWaterResource estateWaterResource = await _queries.Get(deleteDto.Id);
        await _commands.Remove(estateWaterResource);
    }

    public async Task CreateAsync(EstateWaterResourceCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        EstateWaterResource estateWaterResource = _mapper.Map<EstateWaterResource>(createDto);
        if (estateWaterResource == null)
        {
            throw new InvalidDataException();
        }
        await _commands.Add(estateWaterResource);
    }
}
