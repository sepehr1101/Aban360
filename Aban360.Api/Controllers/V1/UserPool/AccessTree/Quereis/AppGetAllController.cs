using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Quereis
{
    [Route("v1/app")]
    public class AppGetAllController : BaseController
    {
        private readonly IAppGetAllHandler _appGetAllHandler;
        public AppGetAllController(IAppGetAllHandler appGetAllHandler)
        {
            _appGetAllHandler = appGetAllHandler;
            _appGetAllHandler.NotNull(nameof(_appGetAllHandler));
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<AppGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var apps = await _appGetAllHandler.Handle(cancellationToken);
            return Ok(apps);
        }
    }
}
