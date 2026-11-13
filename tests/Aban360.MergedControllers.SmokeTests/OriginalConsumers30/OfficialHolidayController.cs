// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.Land;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/official-holiday")]
    public class OfficialHolidayController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfficialHolidayCreateHandler _officialHolidayCreateHandler;
        private readonly IOfficialHolidayUpdateHandler _officialHolidayUpdateHandler;
        private readonly IOfficialHolidayDeleteHandler _officialHolidayDeleteHandler;
        private readonly IOfficialHolidayGetSingleHandler _officialHolidayGetSingleHandler;
        private readonly IOfficialHolidayGetAllHandler _officialHolidayGetAllHandler;

        public OfficialHolidayController(
            IUnitOfWork uow,
            IOfficialHolidayCreateHandler officialHolidayCreateHandler,
            IOfficialHolidayUpdateHandler officialHolidayUpdateHandler,
            IOfficialHolidayDeleteHandler officialHolidayDeleteHandler,
            IOfficialHolidayGetSingleHandler officialHolidayGetSingleHandler,
            IOfficialHolidayGetAllHandler officialHolidayGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _officialHolidayCreateHandler = officialHolidayCreateHandler;
            _officialHolidayCreateHandler.NotNull(nameof(officialHolidayCreateHandler));

            _officialHolidayUpdateHandler = officialHolidayUpdateHandler;
            _officialHolidayUpdateHandler.NotNull(nameof(officialHolidayUpdateHandler));

            _officialHolidayDeleteHandler = officialHolidayDeleteHandler;
            _officialHolidayDeleteHandler.NotNull(nameof(officialHolidayDeleteHandler));

            _officialHolidayGetSingleHandler = officialHolidayGetSingleHandler;
            _officialHolidayGetSingleHandler.NotNull(nameof(officialHolidayGetSingleHandler));

            _officialHolidayGetAllHandler = officialHolidayGetAllHandler;
            _officialHolidayGetAllHandler.NotNull(nameof(officialHolidayGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfficialHolidayCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] OfficialHolidayCreateDto createDto, CancellationToken cancellationToken)
        {
            await _officialHolidayCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] OfficialHolidayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _officialHolidayUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfficialHolidayDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] OfficialHolidayDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _officialHolidayDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfficialHolidayGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var officialHolidays = await _officialHolidayGetSingleHandler.Handle(id, cancellationToken);
            return Ok(officialHolidays);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<OfficialHolidayGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var officialHolidays = await _officialHolidayGetAllHandler.Handle(cancellationToken);
            return Ok(officialHolidays);
        }
    }
}
