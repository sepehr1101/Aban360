using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Commands
{
    [Route("v1/sub-module")]
    public class SubModuleCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubModuleCreateHandler _subModuleCreateHandler;
        public SubModuleCreateController(
            IUnitOfWork uow,
            ISubModuleCreateHandler subModuleCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subModuleCreateHandler = subModuleCreateHandler;
            _subModuleCreateHandler.NotNull(nameof(subModuleCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubModuleCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SubModuleCreateDto createDto, CancellationToken cancellationToken)
        {
            await _subModuleCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
