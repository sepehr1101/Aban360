using Aban360.BlobPool.Application.Features.Taxonomy.Services;
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
        private readonly IDocumentCategoryService _service;
        public DocumentCategoryController(IUnitOfWork uow, IDocumentCategoryService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCategoryCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] DocumentCategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCategoryUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromForm] DocumentCategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCategoryDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] DocumentCategoryDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCategoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var documentCategorys = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(documentCategorys);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentCategoryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var documentCategorys = await _service.GetAllAsync(cancellationToken);
            return Ok(documentCategorys);
        }

        [HttpPost]
        [Route("bill-id")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<DocumentCategoryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ByBillId([FromBody] SearchInput inputDate,CancellationToken cancellationToken)
        {
            var documentCategories = await _service.GetByBillIdAsync(inputDate.Input, cancellationToken);
            return Ok(documentCategories);
        }
    }
}
