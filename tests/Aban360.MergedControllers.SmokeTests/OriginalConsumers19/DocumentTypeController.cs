using Aban360.Api.Controllers.V1;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers19.Aban360.Api.Controllers.V1.BlobController.Taxonomy
{
    [Route("v1/document-type")]
    public class DocumentTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentTypeCreateHandler _documentTypeCreateHandler;
        private readonly IDocumentTypeUpdateHandler _documentTypeUpdateHandler;
        private readonly IDocumentTypeDeleteHandler _documentTypeDeleteHandler;
        private readonly IDocumentTypeGetSingleHandler _documentTypeGetSingleHandler;
        private readonly IDocumentTypeGetAllHandler _documentTypeGetAllHandler;

        public DocumentTypeController(
            IUnitOfWork uow,
            IDocumentTypeCreateHandler documentTypeCreateHandler,
            IDocumentTypeUpdateHandler documentTypeUpdateHandler,
            IDocumentTypeDeleteHandler documentTypeDeleteHandler,
            IDocumentTypeGetSingleHandler documentTypeGetSingleHandler,
            IDocumentTypeGetAllHandler documentTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentTypeCreateHandler = documentTypeCreateHandler;
            _documentTypeCreateHandler.NotNull(nameof(documentTypeCreateHandler));

            _documentTypeUpdateHandler = documentTypeUpdateHandler;
            _documentTypeUpdateHandler.NotNull(nameof(documentTypeUpdateHandler));

            _documentTypeDeleteHandler = documentTypeDeleteHandler;
            _documentTypeDeleteHandler.NotNull(nameof(documentTypeDeleteHandler));

            _documentTypeGetSingleHandler = documentTypeGetSingleHandler;
            _documentTypeGetSingleHandler.NotNull(nameof(documentTypeGetSingleHandler));

            _documentTypeGetAllHandler = documentTypeGetAllHandler;
            _documentTypeGetAllHandler.NotNull(nameof(documentTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] DocumentTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _documentTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromForm] DocumentTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _documentTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] DocumentTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _documentTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var documentTypes = await _documentTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(documentTypes);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var documentTypes = await _documentTypeGetAllHandler.Handle(cancellationToken);
            return Ok(documentTypes);
        }
    }
}
