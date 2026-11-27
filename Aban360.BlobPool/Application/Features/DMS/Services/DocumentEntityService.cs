using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;
using Aban360.BlobPool.Domain.Features.DMS.Dto.Queries;
using Aban360.BlobPool.Domain.Features.DMS.Entities;
using Aban360.BlobPool.Persistence.Features.DMS.Commands.Contracts;
using Aban360.BlobPool.Persistence.Features.DMS.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.BlobPool.Application.Features.DMS.Services;

public interface IDocumentEntityService
{
    Task<DocumentEntityGetDto> GetSingleAsync(long id, CancellationToken cancellationToken);
    Task<ICollection<DocumentEntityGetDto>> GetByBillIdAsync(string billId, CancellationToken cancellationToken);
    Task<ICollection<DocumentEntityGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(DocumentEntityUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(DocumentEntityDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(DocumentEntityCreateDto createDto, Guid documentId, CancellationToken cancellationToken);
}

internal sealed class DocumentEntityService : IDocumentEntityService
{
    private readonly IMapper _mapper;
    private readonly IDocumentEntityQueryService _queries;
    private readonly IValidator<DocumentEntityUpdateDto> _updateValidator;
    private readonly IDocumentEntityCommandService _commands;
    private readonly IValidator<DocumentEntityCreateDto> _createValidator;

    public DocumentEntityService(IMapper mapper, IDocumentEntityQueryService queries, IValidator<DocumentEntityUpdateDto> updateValidator, IDocumentEntityCommandService commands, IValidator<DocumentEntityCreateDto> createValidator)
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

    public async Task<DocumentEntityGetDto> GetSingleAsync(long id, CancellationToken cancellationToken)
    {
        var documentEntity = await _queries.Get(id);
        return _mapper.Map<DocumentEntityGetDto>(documentEntity);
    }

    public async Task<ICollection<DocumentEntityGetDto>> GetByBillIdAsync(string billId, CancellationToken cancellationToken)
    {
        var documentEntity = await _queries.Get(billId);
        //use BlobAddhoc but can i use BlobPool.Domian
        return _mapper.Map<ICollection<DocumentEntityGetDto>>(documentEntity);
    }

    public async Task<ICollection<DocumentEntityGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var documentEntity = await _queries.Get();
        return _mapper.Map<ICollection<DocumentEntityGetDto>>(documentEntity);
    }

    public async Task UpdateAsync(DocumentEntityUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var documentEntity = await _queries.Get(updateDto.Id);
        _mapper.Map(updateDto, documentEntity);
    }

    public async Task DeleteAsync(DocumentEntityDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var documentEntity = await _queries.Get(deleteDto.Id);
        await _commands.Remove(documentEntity);
    }

    public async Task CreateAsync(DocumentEntityCreateDto createDto, Guid documentId, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        DocumentEntity documentEntity = _mapper.Map<DocumentEntity>(createDto);
        documentEntity.DocumentId = documentId;
        await _commands.Add(documentEntity);
    }
}
