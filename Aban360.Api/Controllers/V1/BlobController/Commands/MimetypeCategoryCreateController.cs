using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Commands
{
    [Route("v1/mimetype-category")]
    public class MimetypeCategoryCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMimetypeCategoryCreateHandler _mimetypeCategoryCreateHandler;
        public MimetypeCategoryCreateController(
            IUnitOfWork uow,
            IMimetypeCategoryCreateHandler mimetypeCategoryCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _mimetypeCategoryCreateHandler = mimetypeCategoryCreateHandler;
            _mimetypeCategoryCreateHandler.NotNull(nameof(mimetypeCategoryCreateHandler));
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
    }
}
