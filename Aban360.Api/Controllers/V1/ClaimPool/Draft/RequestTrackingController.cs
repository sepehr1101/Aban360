using Aban360.ClaimPool.Application.Features.Draft.Services;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft
{
    [Route("v1/request-tracking")]
    public class RequestTrackingController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestTrackingService _service;
        public RequestTrackingController(IUnitOfWork uow, IRequestTrackingService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestTrackingCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RequestTrackingCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(CurrentUser,createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestTrackingDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] RequestTrackingDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RequestTrackingGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var RequestTrackings = await _service.GetAllAsync(cancellationToken);
            return Ok(RequestTrackings);
        }

        [HttpPost, HttpGet]
        [Route("by-water-meter/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestTrackingGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByWaterMeter(int id, CancellationToken cancellationToken)
        {
            var RequestTrackings = await _service.GetByWaterMeterIdAsync(id, cancellationToken);
            return Ok(RequestTrackings);
        }
    }
}
