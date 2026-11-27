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

namespace Aban360.BlobPool.Application.Features.Taxonomy.Services;

public interface IDocumentCategoryService
{
    Task<DocumentCategoryGetDto> GetSingleAsync(short id, CancellationToken cancellationToken);
    Task<ICollection<DocumentCategoryGetDto>> GetByBillIdAsync(string billId, CancellationToken cancellationToken);
    Task<ICollection<DocumentCategoryGetDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(DocumentCategoryUpdateDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(DocumentCategoryDeleteDto deleteDto, CancellationToken cancellationToken);
    Task CreateAsync(DocumentCategoryCreateDto createDto, CancellationToken cancellationToken);
}

internal sealed class DocumentCategoryService : IDocumentCategoryService
{
    private readonly IMapper _mapper;
    private readonly IDocumentCategoryQueryService _documentCategoryQueryService;
    private readonly IDocumentEntityQueryService _documentEntityQueryService;
    private readonly IDocumentQueryService _documentQueryService;
    private readonly IValidator<DocumentCategoryUpdateDto> _updateValidator;
    private readonly IDocumentCategoryCommandService _documentCategoryCommandService;
    private readonly IValidator<DocumentCategoryCreateDto> _createValidator;

    public DocumentCategoryService(IMapper mapper, IDocumentCategoryQueryService documentCategoryQueryService, IDocumentEntityQueryService documentEntityQueryService, IDocumentQueryService documentQueryService, IValidator<DocumentCategoryUpdateDto> updateValidator, IDocumentCategoryCommandService documentCategoryCommandService, IValidator<DocumentCategoryCreateDto> createValidator)
    {
        mapper.NotNull(nameof(mapper));
        _mapper = mapper;
        documentCategoryQueryService.NotNull(nameof(documentCategoryQueryService));
        _documentCategoryQueryService = documentCategoryQueryService;
        documentEntityQueryService.NotNull(nameof(documentEntityQueryService));
        _documentEntityQueryService = documentEntityQueryService;
        documentQueryService.NotNull(nameof(documentQueryService));
        _documentQueryService = documentQueryService;
        updateValidator.NotNull(nameof(updateValidator));
        _updateValidator = updateValidator;
        documentCategoryCommandService.NotNull(nameof(documentCategoryCommandService));
        _documentCategoryCommandService = documentCategoryCommandService;
        createValidator.NotNull(nameof(createValidator));
        _createValidator = createValidator;
    }

    public async Task<DocumentCategoryGetDto> GetSingleAsync(short id, CancellationToken cancellationToken)
    {
        var documentCategory = await _documentCategoryQueryService.Get(id);
        return _mapper.Map<DocumentCategoryGetDto>(documentCategory);
    }

    public async Task<ICollection<DocumentCategoryGetDto>> GetByBillIdAsync(string billId, CancellationToken cancellationToken)
    {
        var documentEntities = await _documentEntityQueryService.Get(billId);
        var documentCategories = await _documentQueryService.GetDocoumentCategory(documentEntities.Select(d => d.DocumentId).ToList());
        
        return _mapper.Map<ICollection<DocumentCategoryGetDto>>(documentCategories);
    }

    public async Task<ICollection<DocumentCategoryGetDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var documentCategory = await _documentCategoryQueryService.Get();
        return _mapper.Map<ICollection<DocumentCategoryGetDto>>(documentCategory);
    }

    public async Task UpdateAsync(DocumentCategoryUpdateDto updateDto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
        var documentCategory = await _documentCategoryQueryService.Get(updateDto.Id);
        
        MemoryStream memoryStream = new MemoryStream();
        await (updateDto.Icon).CopyToAsync(memoryStream);
        string base64String = Convert.ToBase64String(memoryStream.ToArray());
        documentCategory.Icon = base64String;
    
        _mapper.Map(updateDto, documentCategory);
    }

    public async Task DeleteAsync(DocumentCategoryDeleteDto deleteDto, CancellationToken cancellationToken)
    {
        var documentCategory = await _documentCategoryQueryService.Get(deleteDto.Id);
        _documentCategoryCommandService.Remove(documentCategory);
    }

    public async Task CreateAsync(DocumentCategoryCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    
    
        var documentCategory = _mapper.Map<DocumentCategory>(createDto);
    
        MemoryStream memoryStream = new MemoryStream();
        await (createDto.Icon).CopyToAsync(memoryStream);
        string base64String = Convert.ToBase64String(memoryStream.ToArray());
        documentCategory.Icon = base64String;
    
        await _documentCategoryCommandService.Add(documentCategory);
    }

}
