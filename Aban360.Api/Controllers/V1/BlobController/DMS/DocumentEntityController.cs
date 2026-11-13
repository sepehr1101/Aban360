using Aban360.BlobPool.Application.Features.DMS.Services;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;
using Aban360.BlobPool.Domain.Features.DMS.Dto.Queries;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.DMS
{
    [Route("v1/document-entity")]
    public class DocumentEntityController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentEntityService _service;
        private readonly IDocumentCreateHandler _documentCreateHandler;

        public DocumentEntityController(IUnitOfWork uow, IDocumentEntityService service, IDocumentCreateHandler documentCreateHandler)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
            documentCreateHandler.NotNull(nameof(documentCreateHandler));
            _documentCreateHandler = documentCreateHandler;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityByDocumentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] DocumentEntityByDocumentCreateDto documentEntityByDocumentCreateDto, CancellationToken cancellationToken)
        {
            Guid documentdId = await _documentCreateHandler.Handle(documentEntityByDocumentCreateDto.documentCreateDto.DocumentFile, documentEntityByDocumentCreateDto.documentCreateDto.DocumentTypeId, documentEntityByDocumentCreateDto.documentCreateDto.Description, cancellationToken);
            await _service.CreateAsync(documentEntityByDocumentCreateDto.documentEntityCreateDto, documentdId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(documentEntityByDocumentCreateDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] DocumentEntityUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] DocumentEntityDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(long id, CancellationToken cancellationToken)
        {
            var documentEntitys = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(documentEntitys);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentEntityGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var documentEntitys = await _service.GetAllAsync(cancellationToken);
            return Ok(documentEntitys);
        }
    }
}
