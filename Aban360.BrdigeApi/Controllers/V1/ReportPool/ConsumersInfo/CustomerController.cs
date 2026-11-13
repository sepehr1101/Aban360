using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;
using Aban360.BrdigeApi.Controllers.V1;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using Microsoft.AspNetCore.Authorization;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerInfoHandler _c0_customerInfoByBillIdHandler;
        private readonly ICustomerInfoWithSameMobileNumberHandler _c1_customerInfoWithSameMobileNumber;
        private readonly IConsumerSummaryGetHandler _c2_consumerSummeryQueryService;
        private readonly ILatestDebtService _c2_latestDebtService;
        private readonly ICustomerValidateByBillIdHandler _c3_customerInfoByBillIdHandler;
        private readonly ICustomerOldInfoHandler _c4_customerOldInfoHandler;
        public CustomerController(ICustomerInfoHandler c0_customerInfoByBillIdHandler, ICustomerInfoWithSameMobileNumberHandler c1_customerInfoWithSameMobileNumber, IConsumerSummaryGetHandler c2_summaryQueryService, ILatestDebtService c2_latestDebtService, ICustomerValidateByBillIdHandler c3_customerInfoByBillIdHandler, ICustomerOldInfoHandler c4_customerOldInfoHandler)
        {
            _c0_customerInfoByBillIdHandler = c0_customerInfoByBillIdHandler;
            _c0_customerInfoByBillIdHandler.NotNull(nameof(c0_customerInfoByBillIdHandler));
            _c1_customerInfoWithSameMobileNumber = c1_customerInfoWithSameMobileNumber;
            _c1_customerInfoWithSameMobileNumber.NotNull(nameof(_c1_customerInfoWithSameMobileNumber));
            _c2_consumerSummeryQueryService = c2_summaryQueryService;
            _c2_consumerSummeryQueryService.NotNull(nameof(_c2_consumerSummeryQueryService));
            _c2_latestDebtService = c2_latestDebtService;
            _c2_latestDebtService.NotNull(nameof(_c2_latestDebtService));
            _c3_customerInfoByBillIdHandler = c3_customerInfoByBillIdHandler;
            _c3_customerInfoByBillIdHandler.NotNull(nameof(c3_customerInfoByBillIdHandler));
            _c4_customerOldInfoHandler = c4_customerOldInfoHandler;
            _c4_customerOldInfoHandler.NotNull(nameof(c4_customerOldInfoHandler));
        }

        [HttpPost]
        [Route("~/v1/customer/info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerInfoByBillIdOutputDto>), StatusCodes.Status200OK)]
        [ActionName("GetByBillId")]
        public async Task<IActionResult> GetByBillIdFromCustomerInfoByBillId0(SearchInput input, CancellationToken cancellationToken)
        {
            CustomerInfoByBillIdOutputDto customerInfo = await _c0_customerInfoByBillIdHandler.Handle(input, cancellationToken);
            return Ok(customerInfo);
        }

        [HttpPost]
        [Route("~/v1/customer/by-mobile")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CustomerInfoWithSameMobileNumberHeaderOutputDto, CustomerInfoWithSameMobileNumberDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw([FromBody] SearchInput input, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerInfoWithSameMobileNumberHeaderOutputDto, CustomerInfoWithSameMobileNumberDataOutputDto> customerInfo = await _c1_customerInfoWithSameMobileNumber.Handle(input.Input, cancellationToken);
            return Ok(customerInfo);
        }

        [HttpPost]
        [Route("~/v1/customer/summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConsumerSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummaryInfo([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            ConsumerSummaryDto summary = await _c2_consumerSummeryQueryService.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("~/v1/customer/latest-debt")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<LatestDebtDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLatestDebt([FromBody] SearchInput searchInput)
        {
            LatestDebtDto latestDebtDto = await _c2_latestDebtService.GetLatestDebt(searchInput.Input);
            return Ok(latestDebtDto);
        }

        [HttpPost]
        [Route("~/v1/customer/validate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerBillIdValidateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Validate(SearchInput input, CancellationToken cancellationToken)
        {
            CustomerBillIdValidateDto customerInfo = await _c3_customerInfoByBillIdHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(customerInfo);
        }

        [HttpPost]
        [Route("~/v1/customer/old-info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerOldInfoOutputDto>), StatusCodes.Status200OK)]
        [ActionName("GetByBillId")]
        public async Task<IActionResult> GetByBillIdFromGetCustomerOldInfo4(CustomerOldInfoInputDto input, CancellationToken cancellationToken)
        {
            CustomerOldInfoOutputDto customerInfo = await _c4_customerOldInfoHandler.Handle(input, cancellationToken);
            return Ok(customerInfo);
        }
    }
}
