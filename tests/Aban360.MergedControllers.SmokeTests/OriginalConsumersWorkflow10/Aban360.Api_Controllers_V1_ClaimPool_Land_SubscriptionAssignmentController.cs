using global::Aban360.Api.Controllers.V1.ClaimPool.Land;
using Microsoft.Extensions.Logging;
using global::Aban360.Api.Controllers.V1;
using global::Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using global::Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using global::Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using global::Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using global::Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using global::Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using global::Aban360.ClaimPool.Persistence.Contexts.Contracts;
using global::Aban360.Common.BaseEntities;
using global::Aban360.Common.Categories.ApiResponse;
using global::Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using global::Aban360.ClaimPool.Application.Features.Land.Services;

namespace BeforeConsumersWorkflow10.Aban360.Api.Controllers.V1.ClaimPool.Land{
    [Route("v1/subscription-assignment")]
    public class SubscriptionAssignmentController : BaseController
    {
        private readonly ICustomerUpdateHandler _customerUpdateHandler;
        private readonly ISubscriptionAssignmentService _subscriptionAssignmentService;
        private readonly IUnitOfWork _unitOfWork;
        public SubscriptionAssignmentController(ICustomerUpdateHandler customerUpdateHandler, ISubscriptionAssignmentService subscriptionAssignmentService, IUnitOfWork unitOfWork)
        {
            _customerUpdateHandler = customerUpdateHandler;
            _customerUpdateHandler.NotNull(nameof(customerUpdateHandler));
            _subscriptionAssignmentService = subscriptionAssignmentService;
            _subscriptionAssignmentService.NotNull(nameof(subscriptionAssignmentService));
            _unitOfWork = unitOfWork;
            _unitOfWork.NotNull(nameof(_unitOfWork));
            _subscriptionAssignmentService.NotNull(nameof(_subscriptionAssignmentService));
        }

        //[HttpGet, HttpPost]
        //[Route("update")]
        //[ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentInputUpdateDto>), StatusCodes.Status200OK)]
        //[AllowAnonymous]
        //public async Task<IActionResult> Update([FromBody] SubscriptionAssignmentInputUpdateDto updateDto, CancellationToken cancellationToken)
        //{
        //    await _subscriptionAssignmentHandler.Handle(updateDto, cancellationToken);
        //    return Ok(updateDto);
        //}
        [HttpGet, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentInputUpdateDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] SubscriptionAssignmentInputUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _customerUpdateHandler.Handle(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("update-by-tracknumber")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentByTrackNumberUpdateDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateByTrackNumber([FromBody] SubscriptionAssignmentByTrackNumberUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _subscriptionAssignmentService.UpdateByTrackNumberAsync(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("single")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle([FromBody] SearchInput input, CancellationToken cancellationToken)
        {
            SubscriptionAssignmentGetDto subscriptionAssignment = await _subscriptionAssignmentService.GetAsync(input.Input, cancellationToken);
            return Ok(subscriptionAssignment);
        }
    }
}
