using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/subscription-assignment")]
    public class SubscriptionAssignmentUpdateController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubscriptionAssignmentUpdateHandler _updateHandler;
        public SubscriptionAssignmentUpdateController(
            IUnitOfWork uow,
            ISubscriptionAssignmentUpdateHandler updateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _updateHandler = updateHandler;
            _updateHandler.NotNull(nameof(_updateHandler));
        }

        [HttpGet, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionAssignmentUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SubscriptionAssignmentUpdateDto updateDto, CancellationToken cancellationToken)
        {
            using (var transactionScope = new TransactionScope(
        TransactionScopeOption.Required,
        new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
        TransactionScopeAsyncFlowOption.Enabled))
            {
                await _updateHandler.Handle(updateDto, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);

                transactionScope.Complete();
            }
    
            //await _updateHandler.Handle(updateDto, cancellationToken);
            //await _uow.SaveChangesAsync(cancellationToken);
            return Ok(updateDto);
        }
    }
}
