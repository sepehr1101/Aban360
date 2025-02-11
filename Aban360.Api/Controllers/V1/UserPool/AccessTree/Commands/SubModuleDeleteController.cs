using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Commands
{
    [Route("v1/sub-module")]
    public class SubModuleDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubModuleDeleteHandler _subModuleDeleteHandler;
        public SubModuleDeleteController(
            IUnitOfWork uow,
            ISubModuleDeleteHandler subModuleDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subModuleDeleteHandler = subModuleDeleteHandler;
            _subModuleDeleteHandler.NotNull(nameof(subModuleDeleteHandler));
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubModuleDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SubModuleDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _subModuleDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
