using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Taxonomy
{
    [Route("v1/document-category")]
    public class DocumentCategoryController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentCategoryCreateHandler _documentCategoryCreateHandler;
        private readonly IDocumentCategoryUpdateHandler _documentCategoryUpdateHandler;
        private readonly IDocumentCategoryDeleteHandler _documentCategoryDeleteHandler;
        private readonly IDocumentCategoryGetSingleHandler _documentCategoryGetSingleHandler;
        private readonly IDocumentCategoryGetAllHandler _documentCategoryGetAllHandler;
        private readonly IDocumentCategoryGetByBillIdHandler _documentCategoryGetBillIdHandler;

        public DocumentCategoryController(
            IUnitOfWork uow,
            IDocumentCategoryCreateHandler documentCategoryCreateHandler,
            IDocumentCategoryUpdateHandler documentCategoryUpdateHandler,
            IDocumentCategoryDeleteHandler documentCategoryDeleteHandler,
            IDocumentCategoryGetSingleHandler documentCategoryGetSingleHandler,
            IDocumentCategoryGetAllHandler documentCategoryGetAllHandler,
            IDocumentCategoryGetByBillIdHandler documentCategoryGetBillIdHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentCategoryCreateHandler = documentCategoryCreateHandler;
            _documentCategoryCreateHandler.NotNull(nameof(documentCategoryCreateHandler));

            _documentCategoryUpdateHandler = documentCategoryUpdateHandler;
            _documentCategoryUpdateHandler.NotNull(nameof(documentCategoryUpdateHandler));

            _documentCategoryDeleteHandler = documentCategoryDeleteHandler;
            _documentCategoryDeleteHandler.NotNull(nameof(documentCategoryDeleteHandler));

            _documentCategoryGetSingleHandler = documentCategoryGetSingleHandler;
            _documentCategoryGetSingleHandler.NotNull(nameof(documentCategoryGetSingleHandler));

            _documentCategoryGetAllHandler = documentCategoryGetAllHandler;
            _documentCategoryGetAllHandler.NotNull(nameof(documentCategoryGetAllHandler));

            _documentCategoryGetBillIdHandler = documentCategoryGetBillIdHandler;
            _documentCategoryGetBillIdHandler.NotNull(nameof(documentCategoryGetBillIdHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCategoryCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] DocumentCategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            await _documentCategoryCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCategoryUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromForm] DocumentCategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _documentCategoryUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCategoryDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] DocumentCategoryDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _documentCategoryDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCategoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var documentCategorys = await _documentCategoryGetSingleHandler.Handle(id, cancellationToken);
            return Ok(documentCategorys);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentCategoryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var documentCategorys = await _documentCategoryGetAllHandler.Handle(cancellationToken);
            return Ok(documentCategorys);
        }

        [HttpPost]
        [Route("bill-id")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentCategoryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ByBillId([FromBody] SearchInput inputDate,CancellationToken cancellationToken)
        {
            var documentCategories = await _documentCategoryGetBillIdHandler.Handle(inputDate.Input, cancellationToken);
            return Ok(documentCategories);
        }
    }
}
