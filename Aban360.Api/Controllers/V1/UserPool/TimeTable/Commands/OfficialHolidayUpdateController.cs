using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Commands
{
    [Route("v4/official-holiday")]
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
