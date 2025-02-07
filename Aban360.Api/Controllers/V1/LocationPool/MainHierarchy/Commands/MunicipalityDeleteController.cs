using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/municipality")]
    public class MunicipalityDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMunicipalityDeleteHandler _municipalityDeleteHandler;
        public MunicipalityDeleteController(
            IUnitOfWork uow,
            IMunicipalityDeleteHandler municipalityDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _municipalityDeleteHandler = municipalityDeleteHandler;
            _municipalityDeleteHandler.NotNull(nameof(municipalityDeleteHandler));
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MunicipalityDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] MunicipalityDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _municipalityDeleteHandler.Handel(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
