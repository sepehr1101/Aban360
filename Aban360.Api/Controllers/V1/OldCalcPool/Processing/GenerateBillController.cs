using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/generate-bill")]
    public class GenerateBillController : BaseController
    {
        private readonly IGenerateBillHandler _generateBillHandler;
        private readonly IFreeGenerateBillHandler _freeGenerateBillHandler;
        private readonly IGenerateBillWithRemainedHandler _generateBillWithRemainedHandler;
        private readonly INowGenerateBillHandler _nowGenerateBillHandler;
        private readonly ISmsOldHandler _smsOldHandler;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ILatestBillSendSmsHandler _latestBillSendSmsHandler;
        private readonly IOpLogCommandService _opLogCommandService;
        public GenerateBillController(
            IGenerateBillHandler generateBillHandler,
            IFreeGenerateBillHandler freeGenerateBillHandler,
            IGenerateBillWithRemainedHandler generateBillWithRemainedHandler,
            INowGenerateBillHandler nowGenerateBillHandler,
            ISmsOldHandler smsOldHandler,
            IBackgroundJobClient backgroundJobClient,
            ILatestBillSendSmsHandler latestBillSendSmsHandler,
            IOpLogCommandService opLogCommandService)
        {
            _generateBillHandler = generateBillHandler;
            _generateBillHandler.NotNull(nameof(generateBillHandler));

            _freeGenerateBillHandler = freeGenerateBillHandler;
            _freeGenerateBillHandler.NotNull(nameof(freeGenerateBillHandler));

            _generateBillWithRemainedHandler = generateBillWithRemainedHandler;
            _generateBillWithRemainedHandler.NotNull(nameof(generateBillWithRemainedHandler));

            _nowGenerateBillHandler = nowGenerateBillHandler;
            _nowGenerateBillHandler.NotNull(nameof(nowGenerateBillHandler));

            _smsOldHandler = smsOldHandler;
            _smsOldHandler.NotNull(nameof(smsOldHandler));

            _backgroundJobClient = backgroundJobClient;
            _backgroundJobClient.NotNull(nameof(backgroundJobClient));

            _latestBillSendSmsHandler = latestBillSendSmsHandler;
            _latestBillSendSmsHandler.NotNull(nameof(latestBillSendSmsHandler));

            _opLogCommandService = opLogCommandService;
            _opLogCommandService.NotNull(nameof(opLogCommandService));
        }

        [HttpPost, HttpGet]
        [Route("calc")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NewBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Calculation(GenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            NewBillOutputDto result = await _generateBillHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("free")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NewBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FreeCalculation(FreeGenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            NewBillOutputDto result = await _freeGenerateBillHandler.Handle(inputDto, string.Empty, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("issue-remained")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillIssueRemainedOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIssueRemained(string billId, CancellationToken cancellationToken)
        {
            BillIssueRemainedOutputDto result = await _generateBillWithRemainedHandler.Handle(billId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("now")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NowGenerateBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> NowCalcuation(NowGenerateBillInputDto inputDto, CancellationToken cancellationToken)
        {
            IAppUser appUser = new AppUserManual();
            string requestOpLogText = string.Format(OpLogLiterals.NowGenerateBillRequestOpLog, inputDto.BillId, inputDto.CurrentMeterNumber, inputDto.MobileNumber);
            await _opLogCommandService.Insert(requestOpLogText, appUser);
            try
            {
                NowGenerateBillOutputDto handleResult = await _nowGenerateBillHandler.Handle(inputDto, appUser, cancellationToken);
                HttpStatusCode statusResult = handleResult.ErrorCode switch
                {
                    (int)NowGenerateBillErrorCodeEnum.Success => HttpStatusCode.OK,
                    (int)NowGenerateBillErrorCodeEnum.Low => HttpStatusCode.RequestedRangeNotSatisfiable,
                    (int)NowGenerateBillErrorCodeEnum.High => HttpStatusCode.RequestedRangeNotSatisfiable,
                    (int)NowGenerateBillErrorCodeEnum.PreNumberHigherThanTodayNumber => HttpStatusCode.RequestedRangeNotSatisfiable,
                    (int)NowGenerateBillErrorCodeEnum.PreNumberEqualTodayNumber => HttpStatusCode.RequestedRangeNotSatisfiable,
                    (int)NowGenerateBillErrorCodeEnum.NotAvailableNow => HttpStatusCode.RequestedRangeNotSatisfiable,
                    (int)NowGenerateBillErrorCodeEnum.InvalidDeletionState => HttpStatusCode.RequestedRangeNotSatisfiable,
                    (int)NowGenerateBillErrorCodeEnum.BadRequest => HttpStatusCode.BadRequest,
                };
                NowGenerateBillOutputDto result = new(handleResult.Message, handleResult.ErrorCode);
                if (result.ErrorCode == (int)NowGenerateBillErrorCodeEnum.Success)
                {
                    await _latestBillSendSmsHandler.Handle(inputDto.BillId, inputDto.MobileNumber, appUser, cancellationToken);
                }
                return CustomeStatus(handleResult, statusResult, handleResult.Message);
            }
            catch
            {
                NowGenerateBillOutputDto result = new("سیستم قادر به محاسبه نیست", (int)NowGenerateBillErrorCodeEnum.NotAvailableNow);
                return CustomeStatus(result, HttpStatusCode.RequestedRangeNotSatisfiable, result.Message);
            }
        }
    }
}
