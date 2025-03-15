using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Commands
{
    [Route("v1/module")]
    public class ModuleUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IModuleUpdateHandler _moduleUpdateHandler;
        public ModuleUpdateController(
            IUnitOfWork uow,
            IModuleUpdateHandler moduleUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _moduleUpdateHandler = moduleUpdateHandler;
            _moduleUpdateHandler.NotNull(nameof(moduleUpdateHandler));
        }

        [HttpPatch, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ModuleUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ModuleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _moduleUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
