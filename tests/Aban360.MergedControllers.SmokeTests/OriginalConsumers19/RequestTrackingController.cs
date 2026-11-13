using Aban360.Api.Controllers.V1;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers19.Aban360.Api.Controllers.V1.ClaimPool.Draft
{
    [Route("v1/request-tracking")]
    public class RequestTrackingController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestTrackingCreateHandler _requestTrackingCreateHandler;
        private readonly IRequestTrackingDeleteHandler _requestTrackingDeleteHandler;
        private readonly IRequestTrackingGetAllHandler _requestTrackingGetAllHandler;
        private readonly IRequestTrackingGetByWaterMeterIdHandler _requestTrackingGetByWaterMeterIdHandler;

        public RequestTrackingController(
            IUnitOfWork unitOfWork,
            IRequestTrackingCreateHandler requestTrackingCreateHandler,
            IRequestTrackingDeleteHandler requestTrackingDeleteHandler,
            IRequestTrackingGetAllHandler requestTrackingGetAllHandler,
            IRequestTrackingGetByWaterMeterIdHandler requestTrackingGetByWaterMeterIdHandler)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.NotNull(nameof(unitOfWork));
            _requestTrackingCreateHandler = requestTrackingCreateHandler;
            _requestTrackingCreateHandler.NotNull(nameof(requestTrackingCreateHandler));
            _requestTrackingDeleteHandler = requestTrackingDeleteHandler;
            _requestTrackingDeleteHandler.NotNull(nameof(requestTrackingDeleteHandler));
            _requestTrackingGetAllHandler = requestTrackingGetAllHandler;
            _requestTrackingGetAllHandler.NotNull(nameof(requestTrackingGetAllHandler));
            _requestTrackingGetByWaterMeterIdHandler = requestTrackingGetByWaterMeterIdHandler;
            _requestTrackingGetByWaterMeterIdHandler.NotNull(nameof(requestTrackingGetByWaterMeterIdHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestTrackingCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RequestTrackingCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestTrackingCreateHandler.Handle(CurrentUser,createDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestTrackingDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] RequestTrackingDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestTrackingDeleteHandler.Handle(deleteDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RequestTrackingGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var RequestTrackings = await _requestTrackingGetAllHandler.Handle(cancellationToken);
            return Ok(RequestTrackings);
        }

        [HttpPost, HttpGet]
        [Route("by-water-meter/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestTrackingGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByWaterMeter(int id, CancellationToken cancellationToken)
        {
            var RequestTrackings = await _requestTrackingGetByWaterMeterIdHandler.Handle(id, cancellationToken);
            return Ok(RequestTrackings);
        }
    }
}
