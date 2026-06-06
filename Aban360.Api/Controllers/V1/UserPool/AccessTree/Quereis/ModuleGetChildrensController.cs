using Aban360.Common.Categories.ApiResponse;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Quereis
{
    [Route("v1/module")]
    public class ModuleGetChildrensController : BaseController
    {
        private readonly IModuleGetChildrensHandler _moduleGetChildrensHandler;
        public ModuleGetChildrensController(IModuleGetChildrensHandler moduleGetChildrensHandler)
        {
            _moduleGetChildrensHandler = moduleGetChildrensHandler;
            _moduleGetChildrensHandler.NotNull(nameof(_moduleGetChildrensHandler));
        }

        [HttpGet, HttpPost]
        [Route("childrens/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ModuleWithSubModuleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChildrens(int id, CancellationToken cancellationToken)
        {
            IEnumerable<ModuleWithSubModuleGetDto> Modules = await _moduleGetChildrensHandler.Handle(id, cancellationToken);
            return Ok(Modules);
        }
    }
}
