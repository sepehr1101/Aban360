using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/cordinal-direction")]
    public class CordinalDirectionDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICordinalDirectionDeleteHandler _cordinalDirectionDeleteHandler;
        public CordinalDirectionDeleteController(
            IUnitOfWork uow,
            ICordinalDirectionDeleteHandler cordinalDirectionDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _cordinalDirectionDeleteHandler = cordinalDirectionDeleteHandler;
            _cordinalDirectionDeleteHandler.NotNull(nameof(cordinalDirectionDeleteHandler));
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CordinalDirectionDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CordinalDirectionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _cordinalDirectionDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
