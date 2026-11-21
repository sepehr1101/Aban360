using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Features.DMS.Queries.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Services;

public interface IDocumentService
{
    Task<Guid> CreateAsync(IFormFile documentFile, short documentTypeId, string? description, CancellationToken cancellationToken);
    Task UpdateAsync(DocumentUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(DocumentDeleteDto deleteDto, CancellationToken cancellationToken);
    Task<ICollection<DocumentGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<DocumentGetDto> GetSingleAsync(Guid id, CancellationToken cancellationToken);
    Task<ICollection<Document>> GetByIdsAsync(ICollection<Guid> ids, CancellationToken cancellationToken);
    Task<ICollection<DocumentGetDto>> GetByBillAndCategoryAsync(short documentCategoryId, string billId, CancellationToken cancellationToken);
}

internal sealed class DocumentService : IDocumentService
{
    private readonly IMapper _mapper;
    private readonly IDocumentCommandService _documentCommandService;
    private readonly IValidator<DocumentCreateDto> _createValidator;
    private readonly IDocumentQueryService _documentQueryService;
    private readonly IValidator<DocumentUpdateDto> _updateValidator;
    private readonly IDocumentEntityQueryService _documentEntityQueryService;
    public DocumentService(
        IMapper mapper,
        IDocumentCommandService documentCommandService,
        IValidator<DocumentCreateDto> validator,
        IDocumentQueryService documentQueryService,
        IValidator<DocumentUpdateDto> updateValidator,
        IDocumentEntityQueryService documentEntityQueryService)
    {
        _mapper = mapper;
        _mapper.NotNull(nameof(_mapper));
        _documentCommandService = documentCommandService;
        _documentCommandService.NotNull(nameof(_documentCommandService));
        _createValidator = validator;
        _createValidator.NotNull(nameof(validator));
        _documentQueryService = documentQueryService;
        _documentQueryService.NotNull(nameof(_documentQueryService));
        _updateValidator = updateValidator;
        _updateValidator.NotNull(nameof(updateValidator));
        _documentQueryService.NotNull(nameof(documentQueryService));
        _documentEntityQueryService = documentEntityQueryService;
        _documentEntityQueryService.NotNull(nameof(_documentEntityQueryService));
        _documentQueryService.NotNull(nameof(_documentEntityQueryService));
    }

    public async Task<Guid> CreateAsync(IFormFile documentFile, short documentTypeId, string? description, CancellationToken cancellationToken)
    {
        DocumentCreateDto documentCreateDto = new DocumentCreateDto()
        {
            DocumentFile = documentFile,
            Description = description,
            DocumentTypeId = documentTypeId,
        };
        var validationResult = await _createValidator.ValidateAsync(documentCreateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Document document = new Document();
        using (MemoryStream memoryStream = new MemoryStream())
        {
            await documentCreateDto.DocumentFile.CopyToAsync(memoryStream);
            document.Id = Guid.NewGuid();
            document.FileRowId = Guid.NewGuid();
            document.Name = Path.GetFileNameWithoutExtension(documentCreateDto.DocumentFile.FileName);
            document.Extension = Path.GetExtension(documentCreateDto.DocumentFile.FileName).TrimStart('.');
            document.SizeInByte = memoryStream.Length;
            document.ContentType = documentCreateDto.DocumentFile.ContentType;
            document.FileContent = memoryStream.ToArray();
            document.CreatedDateTime = DateTime.Now;
            document.Description = documentCreateDto.Description;
            document.IsThumbnail = false;
            //document.ParrentId = documentCreateDto.ParrentId;
            document.DocumentTypeId = documentCreateDto.DocumentTypeId;
        }

        await _documentCommandService.Add(document);
        return document.Id;
    }

    public async Task UpdateAsync(DocumentUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        var document = await _documentQueryService.Get(updateDto.Id);
        MemoryStream memoryStream = new MemoryStream();
        await updateDto.document.CopyToAsync(memoryStream);
        document.FileRowId = Guid.NewGuid(); //ToDo
        document.Name = Path.GetFileNameWithoutExtension(updateDto.document.FileName);
        document.Extension = Path.GetExtension(updateDto.document.FileName).TrimStart('.');
        document.SizeInByte = memoryStream.Length;
        document.ContentType = updateDto.document.ContentType;
        document.FileContent = memoryStream.ToArray();
        document.CreatedDateTime = DateTime.Now;
        document.Description = updateDto.Description;
        document.IsThumbnail = updateDto.IsThumbnail;
        document.ParrentId = updateDto.ParrentId;
        _mapper.Map(updateDto, document);
    }

    public async Task DeleteAsync(DocumentDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var Document = await _documentQueryService.Get(deleteDto.Id);
        await _documentCommandService.Remove(Document);
    }

    public async Task<ICollection<DocumentGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var document = await _documentQueryService.Get();
        return _mapper.Map<ICollection<DocumentGetDto>>(document);
    }

    public async Task<DocumentGetDto> GetSingleAsync(Guid id, CancellationToken cancellationToken)
    {
        var document = await _documentQueryService.Get(id);
        return _mapper.Map<DocumentGetDto>(document);
    }

    public async Task<ICollection<Document>> GetByIdsAsync(ICollection<Guid> ids, CancellationToken cancellationToken)
    {
        var document = await _documentQueryService.Get(ids);
        return document;
    }

    public async Task<ICollection<DocumentGetDto>> GetByBillAndCategoryAsync(short documentCategoryId, string billId, CancellationToken cancellationToken)
    {
        var documentEntities = await _documentEntityQueryService.Get(billId);
        var documents = await _documentQueryService.Get(documentEntities.Select(d => d.DocumentId).ToList(), documentCategoryId);
        return _mapper.Map<ICollection<DocumentGetDto>>(documents);
    }
}
