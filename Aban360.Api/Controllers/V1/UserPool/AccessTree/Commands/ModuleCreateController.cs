using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Commands
{
    [Route("v1/module")]
    public class ModuleCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IModuleCreateHandler _moduleCreateHandler;
        public ModuleCreateController(
            IUnitOfWork uow,
            IModuleCreateHandler moduleCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _moduleCreateHandler = moduleCreateHandler;
            _moduleCreateHandler.NotNull(nameof(moduleCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ModuleCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ModuleCreateDto createDto, CancellationToken cancellationToken)
        {
            await _moduleCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
