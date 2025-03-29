using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/official-holiday")]
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
