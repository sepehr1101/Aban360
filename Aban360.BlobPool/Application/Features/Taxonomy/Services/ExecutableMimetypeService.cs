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

public interface IExecutableMimetypeService
{
    Task<ExecutableMimetypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<ExecutableMimetypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(ExecutableMimetypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(ExecutableMimetypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(ExecutableMimetypeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class ExecutableMimetypeService : IExecutableMimetypeService
{
    private readonly IMapper _mapper;
    private readonly IExecutableMimetypeQueryService _queries;
    private readonly IValidator<ExecutableMimetypeUpdateDto> _updateValidator;
    private readonly IExecutableMimetypeCommandService _commands;
    private readonly IValidator<ExecutableMimetypeCreateDto> _createValidator;

    public ExecutableMimetypeService(IMapper mapper, IExecutableMimetypeQueryService queries, IValidator<ExecutableMimetypeUpdateDto> updateValidator, IExecutableMimetypeCommandService commands, IValidator<ExecutableMimetypeCreateDto> createValidator)
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

    public async Task<ExecutableMimetypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var executableMimetype = await _queries.Get(id);
        return _mapper.Map<ExecutableMimetypeGetDto>(executableMimetype);
    }

    public async Task<ICollection<ExecutableMimetypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var executableMimetype = await _queries.Get();
        return _mapper.Map<ICollection<ExecutableMimetypeGetDto>>(executableMimetype);
    }

    public async Task UpdateAsync(ExecutableMimetypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var executableMimetype = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, executableMimetype);
    }

    public async Task DeleteAsync(ExecutableMimetypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var executableMimetype = await _queries.Get(deleteDto.Id);
        _commands.Remove(executableMimetype);
    }

    public async Task CreateAsync(ExecutableMimetypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var ExecutableMimetype = _mapper.Map<ExecutableMimetype>(createDto);
        await _commands.Add(ExecutableMimetype);
    }
}
