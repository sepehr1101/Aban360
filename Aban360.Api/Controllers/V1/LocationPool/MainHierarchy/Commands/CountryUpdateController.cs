using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/country")]
    public class CountryUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICountryUpdateHandler _countryUpdateHandler;
        public CountryUpdateController(
            IUnitOfWork uow,
            ICountryUpdateHandler countryUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _countryUpdateHandler = countryUpdateHandler;
            _countryUpdateHandler.NotNull(nameof(countryUpdateHandler));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] CountryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _countryUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
