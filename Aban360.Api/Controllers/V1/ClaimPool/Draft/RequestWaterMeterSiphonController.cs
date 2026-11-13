using Aban360.ClaimPool.Application.Features.Draft.Services;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;


using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft
{
    [Route("v1/request-water-meter-siphon")]
    public class RequestWaterMeterSiphonController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestWaterMeterSiphonService _service;

        public RequestWaterMeterSiphonController(IUnitOfWork unitOfWork, IRequestWaterMeterSiphonService service)
        {
            unitOfWork.NotNull(nameof(unitOfWork));
            _unitOfWork = unitOfWork;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterSiphonRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterSiphonRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterSiphonRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterSiphonRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterSiphonRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterSiphonRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
