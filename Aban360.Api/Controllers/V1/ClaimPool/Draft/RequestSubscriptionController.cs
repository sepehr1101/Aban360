using Aban360.ClaimPool.Application.Features.Draft.Services;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft
{
    public class RequestSubscriptionController : BaseController
    {
        private readonly IUnitOfWork _c0_unitOfWork;
        private readonly IRequestSubscriptionCreateHandler _c0_requestSubscriptionCreateHandler;
        private readonly IRequestSubscriptionGetHandler _c0_requestSubscriptionGetHandler;
        private readonly IRequestUserService _service;

        public RequestSubscriptionController(IUnitOfWork c0_unitOfWork, IRequestSubscriptionCreateHandler c0_requestSubscriptionCreateHandler, IRequestSubscriptionGetHandler c0_requestSubscriptionGetHandler, IRequestUserService service)
        {
            c0_unitOfWork.NotNull(nameof(c0_unitOfWork));
            _c0_unitOfWork = c0_unitOfWork;
            c0_requestSubscriptionCreateHandler.NotNull(nameof(c0_requestSubscriptionCreateHandler));
            _c0_requestSubscriptionCreateHandler = c0_requestSubscriptionCreateHandler;
            c0_requestSubscriptionGetHandler.NotNull(nameof(c0_requestSubscriptionGetHandler));
            _c0_requestSubscriptionGetHandler = c0_requestSubscriptionGetHandler;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("~/v1/request-subscription/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestSubscriptionCreateDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] RequestSubscriptionCreateDto requestSubscriptionCreateDto, CancellationToken cancellationToken)
        {
            await _c0_requestSubscriptionCreateHandler.Handle(CurrentUser, requestSubscriptionCreateDto, cancellationToken);
            await _c0_unitOfWork.SaveChangesAsync(cancellationToken);
            return Ok(requestSubscriptionCreateDto);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/request-subscription/get/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestSubscriptionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string billId, CancellationToken cancellationToken)
        {
            var requestSubscription = await _c0_requestSubscriptionGetHandler.Handle(billId, cancellationToken);
            return Ok(requestSubscription);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/request-subscription/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RequestUserQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var RequestUsers = await _service.GetAllAsync(cancellationToken);
            return Ok(RequestUsers);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/request-subscription/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestUserQueryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var RequestUsers = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(RequestUsers);
        }
    }
}
