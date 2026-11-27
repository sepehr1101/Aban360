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

public interface IDocumentTypeService
{
    Task<DocumentTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<DocumentTypeGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(DocumentTypeUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(DocumentTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(DocumentTypeCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class DocumentTypeService : IDocumentTypeService
{
    private readonly IMapper _mapper;
    private readonly IDocumentTypeQueryService _documentTypeQueryService;
    private readonly IValidator<DocumentTypeUpdateDto> _updateValidator;
    private readonly IDocumentTypeCommandService _documentTypeCommandService;
    private readonly IValidator<DocumentTypeCreateDto> _createValidator;

    public DocumentTypeService(IMapper mapper, IDocumentTypeQueryService documentTypeQueryService, IValidator<DocumentTypeUpdateDto> updateValidator, IDocumentTypeCommandService documentTypeCommandService, IValidator<DocumentTypeCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        documentTypeQueryService.NotNull(nameof(documentTypeQueryService));
        _documentTypeQueryService = documentTypeQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        documentTypeCommandService.NotNull(nameof(documentTypeCommandService));
        _documentTypeCommandService = documentTypeCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<DocumentTypeGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var documentType = await _documentTypeQueryService.Get(id);
        return _mapper.Map<DocumentTypeGetDto>(documentType);
    }

    public async Task<ICollection<DocumentTypeGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var documentType = await _documentTypeQueryService.Get();
        return _mapper.Map<ICollection<DocumentTypeGetDto>>(documentType);
    }

    public async Task UpdateAsync(DocumentTypeUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var documentType = await _documentTypeQueryService.Get(updateDto.Id);
    
        MemoryStream memoryStream = new MemoryStream();
        await (updateDto.Icon).CopyToAsync(memoryStream);
        string base64String = Convert.ToBase64String(memoryStream.ToArray());
        documentType.Icon = base64String;
    
        _mapper.Map(updateDto, documentType);
    }

    public async Task DeleteAsync(DocumentTypeDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var documentType = await _documentTypeQueryService.Get(deleteDto.Id);
        _documentTypeCommandService.Remove(documentType);
    }

    public async Task CreateAsync(DocumentTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var documentType = _mapper.Map<DocumentType>(createDto);
    
        MemoryStream memoryStream = new MemoryStream();
        await (createDto.Icon).CopyToAsync(memoryStream);
        string base64String = Convert.ToBase64String(memoryStream.ToArray());
        documentType.Icon = base64String;
    
        await _documentTypeCommandService.Add(documentType);
    }

}
