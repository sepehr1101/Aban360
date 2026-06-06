using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Quereis
{
    [Route("v1/sub-module")]
    public class SubModuleGetChildrensController : BaseController
    {
        private readonly ISubModuleGetChildrensHandler _subModuleGetChildrensHandler;
        public SubModuleGetChildrensController(ISubModuleGetChildrensHandler subModuleGetChildrensHandler)
        {
            _subModuleGetChildrensHandler = subModuleGetChildrensHandler;
            _subModuleGetChildrensHandler.NotNull(nameof(_subModuleGetChildrensHandler));
        }

        [HttpGet, HttpPost]
        [Route("childrens/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SubModuleWithEndPointGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChildrens(int id, CancellationToken cancellationToken)
        {
            IEnumerable<SubModuleWithEndPointGetDto> SubModules = await _subModuleGetChildrensHandler.Handle(id, cancellationToken);
            return Ok(SubModules);
        }
    }
}
