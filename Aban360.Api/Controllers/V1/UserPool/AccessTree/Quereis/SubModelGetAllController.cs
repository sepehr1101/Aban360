using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Quereis
{
    [Route("v1/sub-model")]
    public class SubModelGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubModuleGetAllHandler _subModuleGetAllHandler;
        public SubModelGetAllController(
            IUnitOfWork uow,
            ISubModuleGetAllHandler subModuleGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subModuleGetAllHandler = subModuleGetAllHandler;
            _subModuleGetAllHandler.NotNull(nameof(subModuleGetAllHandler));
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SubModuleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var subModule = await _subModuleGetAllHandler.Handle(cancellationToken);
            return Ok(subModule);
        }
    }
}
