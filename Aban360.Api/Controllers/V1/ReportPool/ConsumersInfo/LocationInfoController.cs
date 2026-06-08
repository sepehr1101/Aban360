using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Geo.Contracts;
using Aban360.ReportPool.Domain.Features.Geo;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/location")]
    public class LocationInfoController : BaseController
    {
        private readonly ILocationInfoGetHandler _locationInfoHandle;
        private readonly ILocationBase64GetHandler _locationBase64Handler;
        private readonly ILocationFileHandler _locationFileHandler;
        public LocationInfoController(
            ILocationInfoGetHandler locationInfoHandle,
            ILocationBase64GetHandler locationBase64Handler,
            ILocationFileHandler locationFileHandler)
        {
            _locationInfoHandle = locationInfoHandle;
            _locationInfoHandle.NotNull(nameof(locationInfoHandle));

            _locationBase64Handler = locationBase64Handler;
            _locationBase64Handler.NotNull(nameof(locationBase64Handler));

            _locationFileHandler = locationFileHandler;
            _locationFileHandler.NotNull(nameof(locationFileHandler));
        }

        [HttpPost]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LocationInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Info(SearchInput searchInput, CancellationToken cancellationToken)
        {
            LocationInfoDto summary = await _locationInfoHandle.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }


        [HttpGet]
        [Route("info-base64/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LocationBase64Dto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBase64Info(string billId, CancellationToken cancellationToken)
        {
            LocationBase64Dto result = await _locationBase64Handler.Handle(billId, CurrentUser, cancellationToken);
            return Ok(result);
        }


        [HttpGet]
        [Route("info-file/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FileContentResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFileInfo(string billId, CancellationToken cancellationToken)
        {
            byte[] bytes = await _locationFileHandler.Handle(billId, CurrentUser, cancellationToken);
            FileContentResult file = new(bytes, "image/png");
            return Ok(file);
        }

    }
}
