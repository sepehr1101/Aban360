using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BeforeBatch151.ClaimPool.Draft
{
    [Route("v1/request-subscription")]
    public class RequestSubscriptionController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestSubscriptionCreateHandler _requestSubscriptionCreateHandler;
        private readonly IRequestSubscriptionGetHandler _requestSubscriptionGetHandler;

        public RequestSubscriptionController(
            IUnitOfWork unitOfWork,
            IRequestSubscriptionCreateHandler requestSubscriptionCreateHandler,
            IRequestSubscriptionGetHandler requestSubscriptionGetHandler)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.NotNull(nameof(unitOfWork));
            _requestSubscriptionCreateHandler = requestSubscriptionCreateHandler;
            _requestSubscriptionCreateHandler.NotNull(nameof(requestSubscriptionCreateHandler));
            _requestSubscriptionGetHandler = requestSubscriptionGetHandler;
            _requestSubscriptionGetHandler.NotNull(nameof(requestSubscriptionGetHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestSubscriptionCreateDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] RequestSubscriptionCreateDto requestSubscriptionCreateDto, CancellationToken cancellationToken)
        {
            await _requestSubscriptionCreateHandler.Handle(CurrentUser,requestSubscriptionCreateDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(requestSubscriptionCreateDto);
        }

        [HttpPost, HttpGet]
        [Route("get/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestSubscriptionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string billId, CancellationToken cancellationToken)
        {
            var requestSubscription = await _requestSubscriptionGetHandler.Handle(billId, cancellationToken);
            return Ok(requestSubscription);
        }
    }
}
