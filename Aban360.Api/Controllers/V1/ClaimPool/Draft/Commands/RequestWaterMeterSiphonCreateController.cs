using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-water-meter-siphon")]
    public class RequestWaterMeterSiphonCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestWaterMeterSiphonCreateHandler _requestWaterMeterSiphonCreateHandler;
        public RequestWaterMeterSiphonCreateController(
            IUnitOfWork uow,
            IRequestWaterMeterSiphonCreateHandler requestWaterMeterSiphonCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestWaterMeterSiphonCreateHandler = requestWaterMeterSiphonCreateHandler;
            _requestWaterMeterSiphonCreateHandler.NotNull(nameof(requestWaterMeterSiphonCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterSiphonRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterSiphonRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterSiphonCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
