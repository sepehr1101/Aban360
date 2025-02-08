using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Commands
{
    [Route("v1/module")]
    public class ModuleDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IModuleDeleteHandler _moduleDeleteHandler;
        public ModuleDeleteController(
            IUnitOfWork uow,
            IModuleDeleteHandler moduleDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _moduleDeleteHandler = moduleDeleteHandler;
            _moduleDeleteHandler.NotNull(nameof(moduleDeleteHandler));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] ModuleDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _moduleDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
