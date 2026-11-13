using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/subscription-assignment")]
    public class SubscriptionAssignmentController : BaseController
    {
        private readonly ICustomerUpdateHandler _customerUpdateHandler;
        private readonly ISubscriptionAssignmentUpdateHandler _subscriptionAssignmentUpdateHandler;
        private readonly ISubscriptionAssignmentByTrackNumberUpdateHandler _subscriptionAssignmentByTrackNumberUpdateHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionAssignmentGetHandler _subscriptionAssignmentGetHandler;

        public SubscriptionAssignmentController(
            ICustomerUpdateHandler customerUpdateHandler,
            ISubscriptionAssignmentUpdateHandler subscriptionAssignmentUpdateHandler,
            ISubscriptionAssignmentByTrackNumberUpdateHandler subscriptionAssignmentByTrackNumberUpdateHandler,
            IUnitOfWork unitOfWork,
            ISubscriptionAssignmentGetHandler subscriptionAssignmentGetHandler)
        {
            _customerUpdateHandler = customerUpdateHandler;
            _customerUpdateHandler.NotNull(nameof(customerUpdateHandler));
            _subscriptionAssignmentUpdateHandler = subscriptionAssignmentUpdateHandler;
            _subscriptionAssignmentUpdateHandler.NotNull(nameof(subscriptionAssignmentUpdateHandler));
            _subscriptionAssignmentByTrackNumberUpdateHandler = subscriptionAssignmentByTrackNumberUpdateHandler;
            _subscriptionAssignmentByTrackNumberUpdateHandler.NotNull(nameof(subscriptionAssignmentByTrackNumberUpdateHandler));
            _unitOfWork = unitOfWork;
            _unitOfWork.NotNull(nameof(_unitOfWork));
            _subscriptionAssignmentGetHandler = subscriptionAssignmentGetHandler;
            _subscriptionAssignmentGetHandler.NotNull(nameof(_subscriptionAssignmentGetHandler));
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
            await _customerUpdateHandler.Handle(updateDto,CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("update-by-tracknumber")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentByTrackNumberUpdateDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateByTrackNumber([FromBody] SubscriptionAssignmentByTrackNumberUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _subscriptionAssignmentByTrackNumberUpdateHandler.Handle(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("single")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle([FromBody]SearchInput input, CancellationToken cancellationToken)
        {
            SubscriptionAssignmentGetDto subscriptionAssignment= await _subscriptionAssignmentGetHandler.Handle(input.Input, cancellationToken);
            return Ok(subscriptionAssignment);
        }
    }
}
