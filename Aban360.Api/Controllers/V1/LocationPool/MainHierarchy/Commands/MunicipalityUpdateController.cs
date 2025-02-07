using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/municipality")]
    public class MunicipalityUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMunicipalityUpdateHandler _municipalityUpdateHandler;
        public MunicipalityUpdateController(
            IUnitOfWork uow,
            IMunicipalityUpdateHandler municipalityUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _municipalityUpdateHandler = municipalityUpdateHandler;
            _municipalityUpdateHandler.NotNull(nameof(municipalityUpdateHandler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MunicipalityUpdateDto>), StatusCodes.Status200OK)]

        public async Task<IActionResult> Update([FromBody] MunicipalityUpdateDto updateDto , CancellationToken cancellationToken)
        {
            await _municipalityUpdateHandler.Handel(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
