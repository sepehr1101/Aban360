using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/official-holiday")]
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
