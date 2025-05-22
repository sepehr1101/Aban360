using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-water-meter")]
    public class RequestWaterMeterCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestWaterMeterCreateHandler _requestWaterMeterCreateHandler;
        public RequestWaterMeterCreateController(
            IUnitOfWork uow,
            IRequestWaterMeterCreateHandler requestWaterMeterCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestWaterMeterCreateHandler = requestWaterMeterCreateHandler;
            _requestWaterMeterCreateHandler.NotNull(nameof(requestWaterMeterCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterCreateHandler.Handle(CurrentUser,createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
