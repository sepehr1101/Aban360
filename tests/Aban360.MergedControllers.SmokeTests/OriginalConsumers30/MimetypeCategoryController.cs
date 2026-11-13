// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.BlobController.Taxonomy;
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

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.BlobController.Taxonomy
{
    [Route("v1/mimetype-category")]
    public class MimetypeCategoryController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMimetypeCategoryCreateHandler _mimetypeCategoryCreateHandler;
        private readonly IMimetypeCategoryUpdateHandler _mimetypeCategoryUpdateHandler;
        private readonly IMimetypeCategoryDeleteHandler _mimetypeCategoryDeleteHandler;
        private readonly IMimetypeCategoryGetSingleHandler _mimetypeCategoryGetSingleHandler;
        private readonly IMimetypeCategoryGetAllHandler _mimetypeCategoryGetAllHandler;

        public MimetypeCategoryController(
            IUnitOfWork uow,
            IMimetypeCategoryCreateHandler mimetypeCategoryCreateHandler,
            IMimetypeCategoryUpdateHandler mimetypeCategoryUpdateHandler,
            IMimetypeCategoryDeleteHandler mimetypeCategoryDeleteHandler,
            IMimetypeCategoryGetSingleHandler mimetypeCategoryGetSingleHandler,
            IMimetypeCategoryGetAllHandler mimetypeCategoryGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _mimetypeCategoryCreateHandler = mimetypeCategoryCreateHandler;
            _mimetypeCategoryCreateHandler.NotNull(nameof(mimetypeCategoryCreateHandler));

            _mimetypeCategoryUpdateHandler = mimetypeCategoryUpdateHandler;
            _mimetypeCategoryUpdateHandler.NotNull(nameof(mimetypeCategoryUpdateHandler));

            _mimetypeCategoryDeleteHandler = mimetypeCategoryDeleteHandler;
            _mimetypeCategoryDeleteHandler.NotNull(nameof(mimetypeCategoryDeleteHandler));

            _mimetypeCategoryGetSingleHandler = mimetypeCategoryGetSingleHandler;
            _mimetypeCategoryGetSingleHandler.NotNull(nameof(mimetypeCategoryGetSingleHandler));

            _mimetypeCategoryGetAllHandler = mimetypeCategoryGetAllHandler;
            _mimetypeCategoryGetAllHandler.NotNull(nameof(mimetypeCategoryGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MimetypeCategoryCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MimetypeCategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            await _mimetypeCategoryCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MimetypeCategoryUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] MimetypeCategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _mimetypeCategoryUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MimetypeCategoryDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] MimetypeCategoryDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _mimetypeCategoryDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MimetypeCategoryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var mimetypeCategorys = await _mimetypeCategoryGetSingleHandler.Handle(id, cancellationToken);
            return Ok(mimetypeCategorys);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<MimetypeCategoryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var mimetypeCategorys = await _mimetypeCategoryGetAllHandler.Handle(cancellationToken);
            return Ok(mimetypeCategorys);
        }
    }
}
