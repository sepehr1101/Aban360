using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Commands
{
    [Route("v1/main-tag-group")]
    public class MainTagGroupGetByIdController : BaseController
    {
        private readonly IMainTagGroupGetHandler _getByIdHandler;

        public MainTagGroupGetByIdController(IMainTagGroupGetHandler getByIdHandler)
        {
            _getByIdHandler = getByIdHandler;
            _getByIdHandler.NotNull(nameof(getByIdHandler));
        }

        [Route("get/{id}")]
        [HttpGet,HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MainTagGroupGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            MainTagGroupGetDto result = await _getByIdHandler.Handle(id);
            return Ok(result);
        }
    }
}
