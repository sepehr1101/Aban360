using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    [Route("v1/subscription-type")]
    public class SubscriptionTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubscriptionTypeCreateHandler _subscriptionTypeCreateHandler;
        private readonly ISubscriptionTypeUpdateHandler _subscriptionTypeUpdateHandler;
        private readonly ISubscriptionTypeDeleteHandler _subscriptionTypeDeleteHandler;
        private readonly ISubscriptionTypeGetSingleHandler _subscriptionTypeGetSingleHandler;
        private readonly ISubscriptionTypeGetAllHandler _subscriptionTypeGetAllHandler;

        public SubscriptionTypeController(
            IUnitOfWork uow,
            ISubscriptionTypeCreateHandler subscriptionTypeCreateHandler,
            ISubscriptionTypeUpdateHandler subscriptionTypeUpdateHandler,
            ISubscriptionTypeDeleteHandler subscriptionTypeDeleteHandler,
            ISubscriptionTypeGetSingleHandler subscriptionTypeGetSingleHandler,
            ISubscriptionTypeGetAllHandler subscriptionTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subscriptionTypeCreateHandler = subscriptionTypeCreateHandler;
            _subscriptionTypeCreateHandler.NotNull(nameof(subscriptionTypeCreateHandler));

            _subscriptionTypeUpdateHandler = subscriptionTypeUpdateHandler;
            _subscriptionTypeUpdateHandler.NotNull(nameof(subscriptionTypeUpdateHandler));

            _subscriptionTypeDeleteHandler = subscriptionTypeDeleteHandler;
            _subscriptionTypeDeleteHandler.NotNull(nameof(subscriptionTypeDeleteHandler));

            _subscriptionTypeGetSingleHandler = subscriptionTypeGetSingleHandler;
            _subscriptionTypeGetSingleHandler.NotNull(nameof(subscriptionTypeGetSingleHandler));

            _subscriptionTypeGetAllHandler = subscriptionTypeGetAllHandler;
            _subscriptionTypeGetAllHandler.NotNull(nameof(subscriptionTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SubscriptionTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _subscriptionTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SubscriptionTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _subscriptionTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SubscriptionTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _subscriptionTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubscriptionTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(SubscriptionTypeEnum id, CancellationToken cancellationToken)
        {
            SubscriptionTypeGetDto subscriptionTypes = await _subscriptionTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(subscriptionTypes);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SubscriptionTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<SubscriptionTypeGetDto> subscriptionTypes = await _subscriptionTypeGetAllHandler.Handle(cancellationToken);
            return Ok(subscriptionTypes);
        }
    }
}
