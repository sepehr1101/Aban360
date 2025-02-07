using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/municipality")]
    public class MunicipalityGetAllController : BaseController
    {
        private readonly IMunicipalityGetAllHandler _municipalityGetAllHandler;
        public MunicipalityGetAllController(IMunicipalityGetAllHandler municipalityGetAllHandler)
        {
            _municipalityGetAllHandler = municipalityGetAllHandler;
            _municipalityGetAllHandler.NotNull(nameof(municipalityGetAllHandler));
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<MunicipalityGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var municipality = await _municipalityGetAllHandler.Handle(cancellationToken);
            return Ok(municipality);
        }
    }
}
