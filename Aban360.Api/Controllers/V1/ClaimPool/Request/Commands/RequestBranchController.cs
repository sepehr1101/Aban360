using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class RequestBranchController : BaseController
    {
        private readonly IKartableRequestGetAllHandler _requestKartableGetAllHandler;
        private readonly IDisplayRequestHandler _displayRequestHandler;
        private readonly IMoshtrakRequestUpdateHandler _moshtrakRequestUpdateHandler;
        private readonly ICloseRequestHandler _closeRequestHandle;
        public RequestBranchController(
            IKartableRequestGetAllHandler requestKartableGetAllHandler,
            IDisplayRequestHandler displayRequestHandler,
            IMoshtrakRequestUpdateHandler moshtrakRequestUpdateHandler,
            ICloseRequestHandler closeRequestHandle)
        {
            _requestKartableGetAllHandler = requestKartableGetAllHandler;
            _requestKartableGetAllHandler.NotNull(nameof(requestKartableGetAllHandler));

            _displayRequestHandler = displayRequestHandler;
            _displayRequestHandler.NotNull(nameof(displayRequestHandler));

            _moshtrakRequestUpdateHandler = moshtrakRequestUpdateHandler;
            _moshtrakRequestUpdateHandler.NotNull(nameof(moshtrakRequestUpdateHandler));

            _closeRequestHandle = closeRequestHandle;
            _closeRequestHandle.NotNull(nameof(closeRequestHandle));
        }


        [HttpGet]
        [Route("kartable")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RequestKartable(CancellationToken cancellationToken)
        {
            ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto> result = await _requestKartableGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }


        [HttpPost]
        [Route("display")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisplayRequest([FromBody] ZoneIdAndTrackNumber inputDto, CancellationToken cancellationToken)
        {
            MoshtrakOutputDto result = await _displayRequestHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }


        [HttpPost]
        [Route("edit")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditRequest([FromBody] MoshtrakUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _moshtrakRequestUpdateHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("close")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestCloseOuputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CloseRequest([FromBody] SearchNumericInput inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            RequestCloseOuputDto result = await _closeRequestHandle.Handle(inputDto.Input, userName, cancellationToken);
            return Ok(result);
        }
    }
}
