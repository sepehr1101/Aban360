using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;

using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Land.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/subscription-assignment")]
    public class SubscriptionAssignmentController : BaseController
    {
        private readonly ICustomerMaintenanceService _customerMaintenanceService;
        private readonly ISubscriptionAssignmentService _subscriptionAssignmentService;
        private readonly IUnitOfWork _unitOfWork;
        public SubscriptionAssignmentController(ICustomerMaintenanceService customerMaintenanceService, ISubscriptionAssignmentService subscriptionAssignmentService, IUnitOfWork unitOfWork)
        {
            _customerMaintenanceService = customerMaintenanceService;
            _customerMaintenanceService.NotNull(nameof(customerMaintenanceService));
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
        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentInputUpdateDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] SubscriptionAssignmentInputUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _customerMaintenanceService.UpdateAsync(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost]
        [Route("update-by-tracknumber")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentByTrackNumberUpdateDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateByTrackNumber([FromBody] SubscriptionAssignmentByTrackNumberUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _subscriptionAssignmentService.UpdateByTrackNumberAsync(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost]
        [Route("single")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle([FromBody] SearchInput input, CancellationToken cancellationToken)
        {
            SubscriptionAssignmentGetDto subscriptionAssignment = await _subscriptionAssignmentService.GetAsync(input.Input, cancellationToken);
            return Ok(subscriptionAssignment);
        }
    }
}
