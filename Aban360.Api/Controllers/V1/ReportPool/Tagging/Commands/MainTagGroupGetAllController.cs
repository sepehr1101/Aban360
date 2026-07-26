using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Commands
{
    [Route("v1/main-tag-group")]
    public class MainTagGroupGetAllController : BaseController
    {
        private readonly IMainTagGroupGetAllHandler _getAllHandler;

        public MainTagGroupGetAllController(IMainTagGroupGetAllHandler getAllHandler)
        {
            _getAllHandler = getAllHandler;
            _getAllHandler.NotNull(nameof(getAllHandler));
        }

        [Route("get")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MainTagGroupGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<MainTagGroupGetDto> result = await _getAllHandler.Handle();
            return Ok(result);
        }
    }
}
