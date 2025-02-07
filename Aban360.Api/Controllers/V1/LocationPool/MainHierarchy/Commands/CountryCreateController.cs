using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/country")]
    public class CountryCreateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICountryCreateHandler _countryCreateHandler;
        public CountryCreateController(
            IUnitOfWork uow,
            ICountryCreateHandler countryCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _countryCreateHandler = countryCreateHandler;
            _countryCreateHandler.NotNull(nameof(countryCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CountryCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CountryCreateDto createDto, CancellationToken cancellationToken)
        {
            await _countryCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
