using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/subscription-assignment")]    
    public class SubscriptionAssignmentGetSingleController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubscriptionAssignmentGetHandler _getSingleHandler;
        public SubscriptionAssignmentGetSingleController(
            IUnitOfWork uow,
            ISubscriptionAssignmentGetHandler getSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _getSingleHandler = getSingleHandler;
            _getSingleHandler.NotNull(nameof(_getSingleHandler));
        }

        [HttpGet, HttpPost]
        [Route("single")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle([FromBody]SearchInput input, CancellationToken cancellationToken)
        {
            SubscriptionAssignmentGetDto subscriptionAssignment= await _getSingleHandler.Handle(input.Input, cancellationToken);
            return Ok(subscriptionAssignment);
        }
    }
}
