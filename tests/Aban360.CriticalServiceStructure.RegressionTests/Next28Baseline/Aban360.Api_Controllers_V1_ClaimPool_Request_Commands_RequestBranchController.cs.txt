using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Request.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class RequestBranchController : BaseController
    {
        private readonly IRequestDisplayService _requestDisplayService;
        private readonly IRequestLifecycleService _requestLifecycleService;
        private readonly IGeneralInformationHandler _generalInformationRequestHandler;
        public RequestBranchController(IRequestDisplayService requestDisplayService, IRequestLifecycleService requestLifecycleService, IGeneralInformationHandler generalInformationRequestHandler)
        {
            _requestDisplayService = requestDisplayService;
            _requestDisplayService.NotNull(nameof(requestDisplayService));
            _requestLifecycleService = requestLifecycleService;
            _requestLifecycleService.NotNull(nameof(requestLifecycleService));
            _generalInformationRequestHandler = generalInformationRequestHandler;
            _generalInformationRequestHandler.NotNull(nameof(generalInformationRequestHandler));
        }

        [HttpPost]
        [Route("edit")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MoshtrakUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditRequest([FromBody] MoshtrakUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _requestLifecycleService.MoshtrakRequestUpdateAsync(inputDto, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("close")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestCloseOuputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CloseRequest([FromBody] CloseRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            RequestCloseOuputDto result = await _requestLifecycleService.CloseRequestAsync(inputDto, userName, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("refer-to")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TrackNumberWithDescriptionInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ToReferRequest([FromBody] TrackNumberWithDescriptionInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _requestLifecycleService.ReferredToIndividualRequestAsync(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("previous-status")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TrackNumberWithDescriptionInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetPreviousStatus([FromBody] TrackNumberWithDescriptionInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _requestLifecycleService.PreviousStatusRequestAsync(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("general/{trackId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GeneralRequestDataOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRequestGeneral(Guid trackId, CancellationToken cancellationToken)
        {
            GeneralRequestDataOutputDto result = await _generalInformationRequestHandler.Handle(trackId, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("previous/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<PreviousRequestDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPreviousRequests(string billId, CancellationToken cancellationToken)
        {
            IEnumerable<PreviousRequestDataOutputDto> result = await _requestDisplayService.PreviousRequestGetByBillIdAsync(billId, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("swap-request-type")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SwapRequestTypeOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SwapRequestType([FromBody] SwapRequestTypeInputDto inputDto, CancellationToken cancellationToken)
        {
            SwapRequestTypeOutputDto resutl = await _requestLifecycleService.SwapRequestTypeAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(resutl);
        }
    }
}
