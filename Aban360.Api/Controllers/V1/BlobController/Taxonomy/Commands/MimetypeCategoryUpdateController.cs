using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Taxonomy.Commands
{
    [Route("v1/mimetype-category")]
    public class MimetypeCategoryUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMimetypeCategoryUpdateHandler _mimetypeCategoryUpdateHandler;
        public MimetypeCategoryUpdateController(
            IUnitOfWork uow,
            IMimetypeCategoryUpdateHandler mimetypeCategoryUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _mimetypeCategoryUpdateHandler = mimetypeCategoryUpdateHandler;
            _mimetypeCategoryUpdateHandler.NotNull(nameof(mimetypeCategoryUpdateHandler));
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
    }
}
