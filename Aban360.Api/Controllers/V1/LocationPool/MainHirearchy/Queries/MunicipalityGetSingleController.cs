using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Queries
{
    [Route("v1/municipality")]
    public class MunicipalityGetSingleController : BaseController
    {
        private readonly IMunicipalityGetSingleHandler _municipalityGetSingleHandler;
        public MunicipalityGetSingleController(IMunicipalityGetSingleHandler municipalityGetSingleHandler)
        {
            _municipalityGetSingleHandler = municipalityGetSingleHandler;
            _municipalityGetSingleHandler.NotNull(nameof(municipalityGetSingleHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MunicipalityGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var municipality = await _municipalityGetSingleHandler.Handle(id, cancellationToken);
            return Ok(municipality);
        }
    }
}
