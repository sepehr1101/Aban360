using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Quereis
{
    [Route("v1/app")]
    public class AppGetChildrensController : BaseController
    {
        private readonly IAppGetChildrensHandler _appGetChildrensHandler;
        public AppGetChildrensController(IAppGetChildrensHandler appGetChildrensHandler)
        {
            _appGetChildrensHandler = appGetChildrensHandler;
            _appGetChildrensHandler.NotNull(nameof(_appGetChildrensHandler));
        }

        [HttpGet, HttpPost]
        [Route("childrens/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<AppWithModuleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChildrens(int id, CancellationToken cancellationToken)
        {
            IEnumerable<AppWithModuleGetDto> apps = await _appGetChildrensHandler.Handle(id, cancellationToken);
            return Ok(apps);
        }
    }
}
