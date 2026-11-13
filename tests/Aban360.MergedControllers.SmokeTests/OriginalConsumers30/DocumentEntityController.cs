// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.BlobController.DMS;
using Aban360.BlobPool.Application.Features.DMS.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Application.Features.DMS.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Application.Features.DMS.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Application.Features.DMS.Handlers.Queries.Contracts;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;
using Aban360.BlobPool.Domain.Features.DMS.Dto.Queries;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.BlobController.DMS
{
    [Route("v1/document-entity")]
    public class DocumentEntityController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentEntityCreateHandler _documentEntityCreateHandler;
        private readonly IDocumentCreateHandler _documentCreateHandler;
        private readonly IDocumentEntityUpdateHandler _documentEntityUpdateHandler;
        private readonly IDocumentEntityDeleteHandler _documentEntityDeleteHandler;
        private readonly IDocumentEntityGetSingleHandler _documentEntityGetSingleHandler;
        private readonly IDocumentEntityGetAllHandler _documentEntityGetAllHandler;

        public DocumentEntityController(
            IUnitOfWork uow,
            IDocumentEntityCreateHandler documentEntityCreateHandler,
            IDocumentCreateHandler documentCreateHandler,
            IDocumentEntityUpdateHandler documentEntityUpdateHandler,
            IDocumentEntityDeleteHandler documentEntityDeleteHandler,
            IDocumentEntityGetSingleHandler documentEntityGetSingleHandler,
            IDocumentEntityGetAllHandler documentEntityGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentEntityCreateHandler = documentEntityCreateHandler;
            _documentEntityCreateHandler.NotNull(nameof(documentEntityCreateHandler));

            _documentCreateHandler = documentCreateHandler;
            _documentCreateHandler.NotNull(nameof(documentCreateHandler));

            _documentEntityUpdateHandler = documentEntityUpdateHandler;
            _documentEntityUpdateHandler.NotNull(nameof(documentEntityUpdateHandler));

            _documentEntityDeleteHandler = documentEntityDeleteHandler;
            _documentEntityDeleteHandler.NotNull(nameof(documentEntityDeleteHandler));

            _documentEntityGetSingleHandler = documentEntityGetSingleHandler;
            _documentEntityGetSingleHandler.NotNull(nameof(documentEntityGetSingleHandler));

            _documentEntityGetAllHandler = documentEntityGetAllHandler;
            _documentEntityGetAllHandler.NotNull(nameof(documentEntityGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityByDocumentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] DocumentEntityByDocumentCreateDto documentEntityByDocumentCreateDto, CancellationToken cancellationToken)
        {
            Guid documentdId = await _documentCreateHandler.Handle(documentEntityByDocumentCreateDto.documentCreateDto.DocumentFile, documentEntityByDocumentCreateDto.documentCreateDto.DocumentTypeId, documentEntityByDocumentCreateDto.documentCreateDto.Description, cancellationToken);
            await _documentEntityCreateHandler.Handle(documentEntityByDocumentCreateDto.documentEntityCreateDto, documentdId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(documentEntityByDocumentCreateDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] DocumentEntityUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _documentEntityUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] DocumentEntityDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _documentEntityDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(long id, CancellationToken cancellationToken)
        {
            var documentEntitys = await _documentEntityGetSingleHandler.Handle(id, cancellationToken);
            return Ok(documentEntitys);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentEntityGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var documentEntitys = await _documentEntityGetAllHandler.Handle(cancellationToken);
            return Ok(documentEntitys);
        }
    }
}
