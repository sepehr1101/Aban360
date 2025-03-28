using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Commands
{
    [Route("v4/official-holiday")]
    public class OfficialHolidayCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfficialHolidayCreateHandler officialHolidayCreateHandler;
        public OfficialHolidayCreateController(
            IUnitOfWork uow,
            IOfficialHolidayCreateHandler officialHolidayCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            this.officialHolidayCreateHandler = officialHolidayCreateHandler;
            this.officialHolidayCreateHandler.NotNull(nameof(officialHolidayCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfficialHolidayCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] OfficialHolidayCreateDto createDto, CancellationToken cancellationToken)
        {
            await officialHolidayCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
