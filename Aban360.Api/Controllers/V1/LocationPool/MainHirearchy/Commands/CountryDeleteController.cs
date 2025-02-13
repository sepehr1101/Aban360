using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Commands
{
    [Route("v1/country")]
    public class CountryDeleteController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICountryDeleteHandler _countryDeleteHandler;
        public CountryDeleteController(
            IUnitOfWork uow, 
            ICountryDeleteHandler countryDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _countryDeleteHandler = countryDeleteHandler;
            _countryDeleteHandler.NotNull(nameof(countryDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CountryDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CountryDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _countryDeleteHandler.Handle(deleteDto,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
