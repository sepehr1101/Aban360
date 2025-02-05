using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/cordinal-direction")]
    public class CordinalDirectionCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICordinalDirectionCreateHandler _cordinalDirectionCreateHandler;
        public CordinalDirectionCreateController(
            IUnitOfWork uow,
            ICordinalDirectionCreateHandler cordinalDirectionCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _cordinalDirectionCreateHandler = cordinalDirectionCreateHandler;
            _cordinalDirectionCreateHandler.NotNull(nameof(cordinalDirectionCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CordinalDirectionCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CordinalDirectionCreateDto createDto, CancellationToken cancellationToken)
        {
            await _cordinalDirectionCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
