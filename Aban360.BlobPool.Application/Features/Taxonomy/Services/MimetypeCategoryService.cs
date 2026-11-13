using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Services;

public interface IMimetypeCategoryService
{
    Task<MimetypeCategoryGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<MimetypeCategoryGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(MimetypeCategoryUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(MimetypeCategoryDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(MimetypeCategoryCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class MimetypeCategoryService : IMimetypeCategoryService
{
    private readonly IMapper _mapper;
    private readonly IMimetypeCategoryQueryService _queries;
    private readonly IValidator<MimetypeCategoryUpdateDto> _updateValidator;
    private readonly IMimetypeCategoryCommandService _commands;
    private readonly IValidator<MimetypeCategoryCreateDto> _createValidator;

    public MimetypeCategoryService(IMapper mapper, IMimetypeCategoryQueryService queries, IValidator<MimetypeCategoryUpdateDto> updateValidator, IMimetypeCategoryCommandService commands, IValidator<MimetypeCategoryCreateDto> createValidator)
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

    public async Task<MimetypeCategoryGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var mimetypeCategory = await _queries.Get(id);
        return _mapper.Map<MimetypeCategoryGetDto>(mimetypeCategory);
    }

    public async Task<ICollection<MimetypeCategoryGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var mimetypeCategory = await _queries.Get();
        return _mapper.Map<ICollection<MimetypeCategoryGetDto>>(mimetypeCategory);
    }

    public async Task UpdateAsync(MimetypeCategoryUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var mimetypeCategory = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, mimetypeCategory);
    }

    public async Task DeleteAsync(MimetypeCategoryDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var mimetypeCategory = await _queries.Get(deleteDto.Id);
        _commands.Remove(mimetypeCategory);
    }

    public async Task CreateAsync(MimetypeCategoryCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var mimetypeCategory = _mapper.Map<MimetypeCategory>(createDto);
        await _commands.Add(mimetypeCategory);
    }
}
