using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
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

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _countryGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

    }
}
