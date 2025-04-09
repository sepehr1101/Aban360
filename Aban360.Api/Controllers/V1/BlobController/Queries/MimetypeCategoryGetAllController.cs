using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Queries
{
    [Route("v1/mimetype_category")]
    public class MimetypeCategoryGetAllController : BaseController
    {
        private readonly IMimetypeCategoryGetAllHandler _mimetypeCategoryGetAllHandler;
        public MimetypeCategoryGetAllController(IMimetypeCategoryGetAllHandler mimetypeCategoryGetAllHandler)
        {
            _mimetypeCategoryGetAllHandler = mimetypeCategoryGetAllHandler;
            _mimetypeCategoryGetAllHandler.NotNull(nameof(mimetypeCategoryGetAllHandler));
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
