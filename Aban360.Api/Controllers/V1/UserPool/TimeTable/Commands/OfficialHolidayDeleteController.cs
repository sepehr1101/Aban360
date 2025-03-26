using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Commands
{
    [Route("v4/official-holiday")]
    public class OfficialHolidayDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfficialHolidayDeleteHandler officialHolidayDeleteHandler;
        public OfficialHolidayDeleteController(
            IUnitOfWork uow,
            IOfficialHolidayDeleteHandler officialHolidayDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            this.officialHolidayDeleteHandler = officialHolidayDeleteHandler;
            this.officialHolidayDeleteHandler.NotNull(nameof(officialHolidayDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfficialHolidayDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] OfficialHolidayDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await officialHolidayDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
