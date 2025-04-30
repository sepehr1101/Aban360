using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Commands
{
    [Route("v1/mimetype-category")]
    public class MimetypeCategoryDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMimetypeCategoryDeleteHandler _mimetypeCategoryDeleteHandler;
        public MimetypeCategoryDeleteController(
            IUnitOfWork uow,
            IMimetypeCategoryDeleteHandler mimetypeCategoryDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _mimetypeCategoryDeleteHandler = mimetypeCategoryDeleteHandler;
            _mimetypeCategoryDeleteHandler.NotNull(nameof(mimetypeCategoryDeleteHandler));
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
    }
}
