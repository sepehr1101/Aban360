using Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.DMS.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.DMS.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.DMS.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.DMS.Commands
{
    [Route("v1/document-entity")]
    public class DocumentEntityCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentEntityCreateHandler _documentEntityCreateHandler;
        public DocumentEntityCreateController(
            IUnitOfWork uow,
            IDocumentEntityCreateHandler documentEntityCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentEntityCreateHandler = documentEntityCreateHandler;
            _documentEntityCreateHandler.NotNull(nameof(documentEntityCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] DocumentEntityCreateDto createDto, CancellationToken cancellationToken)
        {
            await _documentEntityCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
    [Route("v1/document-entity")]
    public class DocumentEntityDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentEntityDeleteHandler _documentEntityDeleteHandler;
        public DocumentEntityDeleteController(
            IUnitOfWork uow,
            IDocumentEntityDeleteHandler documentEntityDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentEntityDeleteHandler = documentEntityDeleteHandler;
            _documentEntityDeleteHandler.NotNull(nameof(documentEntityDeleteHandler));
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
    }
    [Route("v1/document-entity")]
    public class DocumentEntityUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentEntityUpdateHandler _documentEntityUpdateHandler;
        public DocumentEntityUpdateController(
            IUnitOfWork uow,
            IDocumentEntityUpdateHandler documentEntityUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentEntityUpdateHandler = documentEntityUpdateHandler;
            _documentEntityUpdateHandler.NotNull(nameof(documentEntityUpdateHandler));
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
    }
    [Route("v1/document-entity")]
    public class DocumentEntityGetAllController : BaseController
    {
        private readonly IDocumentEntityGetAllHandler _documentEntityGetAllHandler;
        public DocumentEntityGetAllController(IDocumentEntityGetAllHandler documentEntityGetAllHandler)
        {
            _documentEntityGetAllHandler = documentEntityGetAllHandler;
            _documentEntityGetAllHandler.NotNull(nameof(documentEntityGetAllHandler));
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
    [Route("v1/document-entity")]
    public class DocumentEntityGetSingleController : BaseController
    {
        private readonly IDocumentEntityGetSingleHandler _documentEntityGetSingleHandler;
        public DocumentEntityGetSingleController(IDocumentEntityGetSingleHandler documentEntityGetSingleHandler)
        {
            _documentEntityGetSingleHandler = documentEntityGetSingleHandler;
            _documentEntityGetSingleHandler.NotNull(nameof(documentEntityGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(long id, CancellationToken cancellationToken)
        {
            var documentEntitys = await _documentEntityGetSingleHandler.Handle(id, cancellationToken);
            return Ok(documentEntitys);
        }
    }
}
