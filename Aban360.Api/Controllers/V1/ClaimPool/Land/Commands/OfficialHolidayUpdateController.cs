using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/official-holiday")]
    public class OfficialHolidayUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfficialHolidayUpdateHandler officialHolidayUpdateHandler;
        public OfficialHolidayUpdateController(
            IUnitOfWork uow,
            IOfficialHolidayUpdateHandler officialHolidayUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            this.officialHolidayUpdateHandler = officialHolidayUpdateHandler;
            this.officialHolidayUpdateHandler.NotNull(nameof(officialHolidayUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] OfficialHolidayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await officialHolidayUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
