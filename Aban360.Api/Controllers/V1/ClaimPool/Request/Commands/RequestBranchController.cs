using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class RequestBranchController : BaseController
    {
        private readonly IKartableRequestGetAllHandler _requestKartableGetAllHandler;
        private readonly IDisplayRequestHandler _displayRequestHandler;
        private readonly IMoshtrakRequestUpdateHandler _moshtrakRequestUpdateHandler;
        private readonly IRequestNewBranchHandler _requestNewBranchHandler;
        private readonly IRequestAfterSaleHandler _requestAfterSaleHandler;
        private readonly IRequestDuplicateValidation _requestDuplicateValidation;
        private readonly ICloseRequestHandler _closeRequestHandle;
        public RequestBranchController(
            IKartableRequestGetAllHandler requestKartableGetAllHandler,
            IDisplayRequestHandler displayRequestHandler,
            IMoshtrakRequestUpdateHandler moshtrakRequestUpdateHandler,
            IRequestNewBranchHandler requestNewBranchHandler,
            IRequestAfterSaleHandler requestAfterSaleHandler,
            IRequestDuplicateValidation requestDuplicateValidation,
            ICloseRequestHandler closeRequestHandle)
        {
            _requestKartableGetAllHandler = requestKartableGetAllHandler;
            _requestKartableGetAllHandler.NotNull(nameof(requestKartableGetAllHandler));

            _displayRequestHandler = displayRequestHandler;
            _displayRequestHandler.NotNull(nameof(displayRequestHandler));

            _moshtrakRequestUpdateHandler = moshtrakRequestUpdateHandler;
            _moshtrakRequestUpdateHandler.NotNull(nameof(requestNewBranchHandler));

            _requestNewBranchHandler = requestNewBranchHandler;
            _requestNewBranchHandler.NotNull(nameof(requestNewBranchHandler));

            _requestAfterSaleHandler = requestAfterSaleHandler;
            _requestAfterSaleHandler.NotNull(nameof(requestAfterSaleHandler));

            _requestDuplicateValidation = requestDuplicateValidation;
            _requestDuplicateValidation.NotNull(nameof(requestDuplicateValidation));

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
        [Route("new")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestNewBranchInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> NewRequest([FromBody] RequestNewBranchInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = GetUserCode();
            await _requestNewBranchHandler.Handle(inputDto, userCode, cancellationToken);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("a-s")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestAfterSaleInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AfterSaleRequest([FromBody] RequestAfterSaleInputDto inputDto, CancellationToken cancellationToken)
        {
            int userCode = GetUserCode();
            await _requestAfterSaleHandler.Handle(inputDto, userCode, cancellationToken);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("is-duplicate/new")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TrackingDuplicateValidationOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsDuplicateNewRequest([FromBody] NewTrackingDuplicateValidationInputDto inputDto, CancellationToken cancellationToken)
        {
            TrackingDuplicateValidationInputDto totalValidation = new(null, inputDto.NeighbourBillId, inputDto.NationalCode, TrackingDuplicateValidationTypeEnum.ByNationalCode);
            TrackingDuplicateValidationOutputDto result = await _requestDuplicateValidation.Handle(totalValidation, cancellationToken);
            return Ok(result);
        }


        [HttpPost]
        [Route("is-duplicate/a-s")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TrackingDuplicateValidationOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsDuplicateAfterSaleRequest([FromBody] AfterSaleTrackingDuplicateValidationInputDto inputDto, CancellationToken cancellationToken)
        {
            TrackingDuplicateValidationInputDto totalValidation = new(inputDto.BillId, null, null, TrackingDuplicateValidationTypeEnum.ByBillId);
            TrackingDuplicateValidationOutputDto result = await _requestDuplicateValidation.Handle(totalValidation, cancellationToken);
            return Ok(result);
        }


        [HttpPost]
        [Route("close")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestCloseOuputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CloseRequest([FromBody] SearchInput inputDto, CancellationToken cancellationToken)
        {
            RequestCloseOuputDto result = await _closeRequestHandle.Handle(int.Parse(inputDto.Input), cancellationToken);
            return Ok(result);
        }

        private int GetUserCode()
        {
            bool isSuccess = int.TryParse(CurrentUser.Username, out int userCode);
            if (!isSuccess)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidUserName);
            }

            return userCode;
        }
    }
}
