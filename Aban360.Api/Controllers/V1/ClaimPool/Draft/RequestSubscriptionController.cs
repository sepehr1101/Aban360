using Aban360.ClaimPool.Application.Features.Draft.Services;
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
    [Route("v1/request-subscription")]
    public class RequestSubscriptionController : BaseController
    {
        private readonly IUnitOfWork _c0_unitOfWork;
        private readonly IRequestSubscriptionService _requestSubscriptionService;
        private readonly IRequestUserService _service;
        public RequestSubscriptionController(IUnitOfWork c0_unitOfWork, IRequestSubscriptionService requestSubscriptionService, IRequestUserService service)
        {
            c0_unitOfWork.NotNull(nameof(c0_unitOfWork));
            _c0_unitOfWork = c0_unitOfWork;
            requestSubscriptionService.NotNull(nameof(requestSubscriptionService));
            _requestSubscriptionService = requestSubscriptionService;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestSubscriptionCreateDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] RequestSubscriptionCreateDto requestSubscriptionCreateDto, CancellationToken cancellationToken)
        {
            await _requestSubscriptionService.CreateAsync(CurrentUser, requestSubscriptionCreateDto, cancellationToken);
            await _c0_unitOfWork.SaveChangesAsync(cancellationToken);
            return Ok(requestSubscriptionCreateDto);
        }

        [HttpPost, HttpGet]
        [Route("get/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestSubscriptionGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string billId, CancellationToken cancellationToken)
        {
            var requestSubscription = await _requestSubscriptionService.GetAsync(billId, cancellationToken);
            return Ok(requestSubscription);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RequestUserQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var RequestUsers = await _service.GetAllAsync(cancellationToken);
            return Ok(RequestUsers);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestUserQueryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var RequestUsers = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(RequestUsers);
        }
    }
}
