using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-water-meter-tag")]
    public class RequestWaterMeterTagCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestWaterMeterTagCreateHandler _requestWaterMeterTagCreateHandler;
        public RequestWaterMeterTagCreateController(
            IUnitOfWork uow,
            IRequestWaterMeterTagCreateHandler requestWaterMeterTagCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestWaterMeterTagCreateHandler = requestWaterMeterTagCreateHandler;
            _requestWaterMeterTagCreateHandler.NotNull(nameof(requestWaterMeterTagCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterTagRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterTagCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
