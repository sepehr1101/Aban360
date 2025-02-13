using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/cordinal-direction")]
    public class CordinalDirectionUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICordinalDirectionUpdateHandler _cordinalDirectionUpdateHandler;
        public CordinalDirectionUpdateController(
            IUnitOfWork uow,
            ICordinalDirectionUpdateHandler cordinalDirectionUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _cordinalDirectionUpdateHandler = cordinalDirectionUpdateHandler;
            _cordinalDirectionUpdateHandler.NotNull(nameof(cordinalDirectionUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CordinalDirectionDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CordinalDirectionUpdateDto updatedto, CancellationToken cancellationToken)
        {
            await _cordinalDirectionUpdateHandler.Handle(updatedto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updatedto);
        }
    }
}
