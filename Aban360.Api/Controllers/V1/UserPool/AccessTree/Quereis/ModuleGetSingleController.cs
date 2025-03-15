using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Quereis
{
    [Route("v1/module")]
    public class ModuleGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IModuleGetSingleHandler _moduleGetSingleHandler;
        public ModuleGetSingleController(
            IUnitOfWork uow,
            IModuleGetSingleHandler moduleGetSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _moduleGetSingleHandler = moduleGetSingleHandler;
            _moduleGetSingleHandler.NotNull(nameof(moduleGetSingleHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ModuleGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Single(int id, CancellationToken cancellationToken)
        {
            ModuleGetDto module = await _moduleGetSingleHandler.Handle(id, cancellationToken);
            return Ok(module);
        }
    }
}
