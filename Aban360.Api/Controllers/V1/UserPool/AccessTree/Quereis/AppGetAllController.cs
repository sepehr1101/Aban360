using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
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
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var apps = await _appGetAllHandler.Handle(cancellationToken);
            return Ok(apps);
        }
    }
}
