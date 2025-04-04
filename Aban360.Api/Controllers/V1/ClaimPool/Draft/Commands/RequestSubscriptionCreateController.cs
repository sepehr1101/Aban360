using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-subscription")]
    public class RequestSubscriptionCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestSubscriptionCreateHandler _requestUserCreateHandler;
        public RequestSubscriptionCreateController(
            IUnitOfWork uow,
            IRequestSubscriptionCreateHandler requestUserCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestUserCreateHandler = requestUserCreateHandler;
            _requestUserCreateHandler.NotNull(nameof(requestUserCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestSubscriptionCreateDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] RequestSubscriptionCreateDto requestSubscriptionCreateDto, CancellationToken cancellationToken)
        {           
            await _requestUserCreateHandler.Handle(requestSubscriptionCreateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(requestSubscriptionCreateDto);
        }
    }
}
