using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Commands
{
    [Route("v1/sub-model")]
    public class SubModelUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubModuleUpdateHandler _subModuleUpdateHandler;
        public SubModelUpdateController(
            IUnitOfWork uow,
            ISubModuleUpdateHandler subModuleUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subModuleUpdateHandler = subModuleUpdateHandler;
            _subModuleUpdateHandler.NotNull(nameof(subModuleUpdateHandler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubModuleUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SubModuleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _subModuleUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
