using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Queries
{
    [Route("v1/country")]
    public class CountryGetAllController : BaseController
    {
        private readonly ICountryGetAllHandler _countryGetAllHandler;
        public CountryGetAllController(ICountryGetAllHandler countryGetAllHandler)
        {
            _countryGetAllHandler = countryGetAllHandler;
            _countryGetAllHandler.NotNull(nameof(countryGetAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CountryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _countryGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

    }
}
