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

public interface ISiphonTypeService
{
    Task<SiphonTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<SiphonTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(SiphonTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(SiphonTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(SiphonTypeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class SiphonTypeService : ISiphonTypeService
{
    private readonly IMapper _mapper;
    private readonly ISiphonTypeQueryService _queries;
    private readonly IValidator<SiphonTypeUpdateDto> _updateValidator;
    private readonly ISiphonTypeCommandService _commands;
    private readonly IValidator<SiphonTypeCreateDto> _createValidator;

    public SiphonTypeService(IMapper mapper, ISiphonTypeQueryService queries, IValidator<SiphonTypeUpdateDto> updateValidator, ISiphonTypeCommandService commands, IValidator<SiphonTypeCreateDto> createValidator)
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

    public async Task<SiphonTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        SiphonType siphonType = await _queries.Get(id);
        return _mapper.Map<SiphonTypeGetDto>(siphonType);
    }

    public async Task<ICollection<SiphonTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        ICollection<SiphonType> siphonType = await _queries.Get();
        return _mapper.Map<ICollection<SiphonTypeGetDto>>(siphonType);
    }

    public async Task UpdateAsync(SiphonTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        SiphonType siphonType = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, siphonType);
    }

    public async Task DeleteAsync(SiphonTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        SiphonType siphonType = await _queries.Get(deleteDto.Id);
        await _commands.Remove(siphonType);
    }

    public async Task CreateAsync(SiphonTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        SiphonType SiphonType = _mapper.Map<SiphonType>(createDto);
        await _commands.Add(SiphonType);
    }
}
