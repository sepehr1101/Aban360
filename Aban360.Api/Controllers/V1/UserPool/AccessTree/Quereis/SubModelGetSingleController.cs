using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Quereis
{
    [Route("v1/sub-model")]
    public class SubModelGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubModuleGetSingleHandler _subModuleGetSingleHandler;
        public SubModelGetSingleController(
            IUnitOfWork uow,
            ISubModuleGetSingleHandler subModuleGetSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subModuleGetSingleHandler = subModuleGetSingleHandler;
            _subModuleGetSingleHandler.NotNull(nameof(subModuleGetSingleHandler));
        }

        [HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubModuleGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Single(int id, CancellationToken cancellationToken)
        {
            var subModule = await _subModuleGetSingleHandler.Handle(id, cancellationToken);
            return Ok(subModule);
        }
    }
}
