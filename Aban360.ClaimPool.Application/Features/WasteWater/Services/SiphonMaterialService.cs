using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Services;

public interface ISiphonMaterialService
{
    Task<SiphonMaterialGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<SiphonMaterialGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(SiphonMaterialUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(SiphonMaterialDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(SiphonMaterialCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class SiphonMaterialService : ISiphonMaterialService
{
    private readonly IMapper _mapper;
    private readonly ISiphonMaterialQueryService _queries;
    private readonly IValidator<SiphonMaterialUpdateDto> _updateValidator;
    private readonly ISiphonMaterialCommandService _commands;
    private readonly IValidator<SiphonMaterialCreateDto> _createValidator;

    public SiphonMaterialService(IMapper mapper, ISiphonMaterialQueryService queries, IValidator<SiphonMaterialUpdateDto> updateValidator, ISiphonMaterialCommandService commands, IValidator<SiphonMaterialCreateDto> createValidator)
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

    public async Task<SiphonMaterialGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        SiphonMaterial siphonMaterial = await _queries.Get(id);
        return _mapper.Map<SiphonMaterialGetDto>(siphonMaterial);
    }

    public async Task<ICollection<SiphonMaterialGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<SiphonMaterial> siphonMaterial = await _queries.Get();
        return _mapper.Map<ICollection<SiphonMaterialGetDto>>(siphonMaterial);
    }

    public async Task UpdateAsync(SiphonMaterialUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        SiphonMaterial siphonMaterial = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, siphonMaterial);
    }

    public async Task DeleteAsync(SiphonMaterialDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        SiphonMaterial siphonMaterial = await _queries.Get(deleteDto.Id);
        await _commands.Remove(siphonMaterial);
    }

    public async Task CreateAsync(SiphonMaterialCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        SiphonMaterial SiphonMaterial = _mapper.Map<SiphonMaterial>(createDto);
        await _commands.Add(SiphonMaterial);
    }
}
