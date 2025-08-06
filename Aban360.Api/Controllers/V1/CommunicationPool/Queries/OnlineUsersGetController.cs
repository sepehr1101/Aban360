using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.CommunicationPool.Application.Features.Hubs.Handlers;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Queries;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CommunicationPool.Queries
{
    [Route("v1/online-user")]
    public class OnlineUsersGetController : BaseController
    {
        private readonly IOnlineUserGetHandler _onlineUserGetHandler;
        public OnlineUsersGetController(IOnlineUserGetHandler onlineUserGetHandler)
        {
            _onlineUserGetHandler = onlineUserGetHandler;
            _onlineUserGetHandler.NotNull(nameof(onlineUserGetHandler));
        }


        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<OnlineUserGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<OnlineUserGetDto> result = await _onlineUserGetHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}