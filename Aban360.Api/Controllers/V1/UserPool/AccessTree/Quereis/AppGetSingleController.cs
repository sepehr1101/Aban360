using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Quereis
{
    [Route("v1/app")]
    public class AppGetSingleController : BaseController
    {
        private readonly IAppGetSingleHandler _appGetSingleHandler;
        public AppGetSingleController(IAppGetSingleHandler appGetSingleHandler)
        {
            _appGetSingleHandler = appGetSingleHandler;
            _appGetSingleHandler.NotNull(nameof(_appGetSingleHandler));
        }

        [HttpGet]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var app = await _appGetSingleHandler.Handle(id, cancellationToken);
            return Ok(app);
        }
    }
}
