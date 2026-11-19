using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.BaseEntities;
using Microsoft.AspNetCore.Authorization;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.BrdigeApi.Controllers.V1.CalculationPool;
using Aban360.BrdigeApi.Controllers.V1.ServiceLink;
using Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo;
using Aban360.OldCalcPool.Application.Features.Db70.Services;
using Aban360.BrdigeApi.Controllers.V1;

namespace BeforeConsumersOperations19.Aban360.BrdigeApi.Controllers.V1
{
    [Route("v1/service-link")]
    public class ServiceLinkController : BaseController
    {
        private readonly IServiceLinkInquiryDetailHandler _c0_serviceLinkInquiryDetailHandler;
        private readonly IServiceLinkInstallmentHandler _c1_serviceLinkInstallmentHandler;
        private readonly IServiceLinkRegisterManualHandler _c3_serviceLinkRegisterHandler;
        private readonly IPaymentInquiryHandler _c6_paymentInquiryHandler;
        private readonly IOfferingService _offeringService;
        private readonly IConnectDisconnectSetResultHandler _c8_connectDisconnectSetResultHandler;
        string _c8_successfullyDone = "با موفقیت انجام شد";
        int _c8_disconnectState = 5;
        int _c8_connectState = 0;
        public ServiceLinkController(IServiceLinkInquiryDetailHandler c0_serviceLinkInquiryDetailHandler, IServiceLinkInstallmentHandler c1_serviceLinkInstallmentHandler, IServiceLinkRegisterManualHandler c3_serviceLinkRegisterHandler, IPaymentInquiryHandler c6_paymentInquiryHandler, IOfferingService offeringService, IConnectDisconnectSetResultHandler c8_connectDisconnectSetResultHandler)
        {
            _c0_serviceLinkInquiryDetailHandler = c0_serviceLinkInquiryDetailHandler;
            _c0_serviceLinkInquiryDetailHandler.NotNull(nameof(c0_serviceLinkInquiryDetailHandler));
            _c1_serviceLinkInstallmentHandler = c1_serviceLinkInstallmentHandler;
            _c1_serviceLinkInstallmentHandler.NotNull(nameof(c1_serviceLinkInstallmentHandler));
            _c3_serviceLinkRegisterHandler = c3_serviceLinkRegisterHandler;
            _c3_serviceLinkRegisterHandler.NotNull(nameof(c3_serviceLinkRegisterHandler));
            _c6_paymentInquiryHandler = c6_paymentInquiryHandler;
            _c6_paymentInquiryHandler.NotNull(nameof(c6_paymentInquiryHandler));
            _offeringService = offeringService;
            _offeringService.NotNull(nameof(offeringService));
            _c8_connectDisconnectSetResultHandler = c8_connectDisconnectSetResultHandler;
            _c8_connectDisconnectSetResultHandler.NotNull(nameof(c8_connectDisconnectSetResultHandler));
        }

        [HttpPost, HttpGet]
        [Route("inquiry")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ServiceLinkInquiryOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> InquaryDetail(ServiceLinkInquiryInputDto inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<ServiceLinkInquiryOutputDto> detail = await _c0_serviceLinkInquiryDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(detail);
        }

        [HttpPost]
        [Route("installment")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Installment([FromBody] ServiceLinkInstallmentInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto> result = await _c1_serviceLinkInstallmentHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("modify-amount")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ModifyAmount(ServiceLinkModifyAmountInputDto inputDto, CancellationToken cancellationToken)
        {
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("register-manual")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServiceLinkRegisterManualInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ManualRegister(ServiceLinkRegisterManualInputDto inputDto, CancellationToken cancellationToken)
        {
            await _c3_serviceLinkRegisterHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet]
        [Route("transaction-type")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransactionTypes(CancellationToken cancellationToken)
        {
            ICollection<NumericDictionary> result = new List<NumericDictionary>()
            {
                new NumericDictionary(1, "بدهکار"),
                new NumericDictionary(2, "بستانکار")
            };
            return Ok(result);
        }

        [HttpPost]
        [Route("connection-fee")]
        [AllowAnonymous]
        public IActionResult CalculateConnectionFee()
        {
            Fee fee = new()
            {
                FeeItems = new[]
                {
                    new FeeItem(27_000_000, 1, "حق انشعاب")
                },
                AmountSum = 27_000_000
            };
            return Ok(fee);
        }

        [HttpPost, HttpGet]
        [Route("check-pay")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<PaymentInquiryOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> checkPay(PaymentInquiryInputDto input, CancellationToken cancellationToken)
        {
            PaymentInquiryOutputDto PaymentInquiry = await _c6_paymentInquiryHandler.Handle(input, cancellationToken);
            return Ok(PaymentInquiry);
        }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItems(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _offeringService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("disconnect")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Disconnect([FromBody] ServiceLinkConnectionInput input, CancellationToken cancellationToken)
        {
            await _c8_connectDisconnectSetResultHandler.Handle(input, false, CurrentUser, cancellationToken);
            return Ok(_c8_successfullyDone);
        }

        [HttpPost]
        [Route("reconnect")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Reconnect([FromBody] ServiceLinkConnectionInput input, CancellationToken cancellationToken)
        {
            await _c8_connectDisconnectSetResultHandler.Handle(input, true, CurrentUser, cancellationToken);
            return Ok(_c8_successfullyDone);
        }
    }
}

namespace Aban360.BrdigeApi.Controllers.V1.ServiceLink
{
    public record Fee
    {
        public ICollection<FeeItem> FeeItems { get; set; } = default!;
        public long AmountSum { get; set; }
    }

    public record FeeItem
    {
        public long Amount { get; set; }
        public int ItemId { get; set; }
        public string ItemTitle { get; set; } = default!;

        public FeeItem(long amount, int itemId, string itemTitle)
        {
            Amount = amount;
            ItemId = itemId;
            ItemTitle = itemTitle;
        }
    }
}
