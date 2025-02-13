using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1._CombinedPools.Queries
{
    [Route("access-tree")]
    [ApiController]
    public class AccessTreeValueKeyQueryController : BaseController
    {
        private readonly IAccessTreeValueKeyQueryHandler _accessTreeValueKeyQueryHandler;
        public AccessTreeValueKeyQueryController(
            IAccessTreeValueKeyQueryHandler accessTreeValueKeyQueryHandler)
        {
            _accessTreeValueKeyQueryHandler = accessTreeValueKeyQueryHandler;
            _accessTreeValueKeyQueryHandler.NotNull(nameof(accessTreeValueKeyQueryHandler));
        }

        [AllowAnonymous]
        [HttpGet,HttpPost]
        [Route("raw")]
        public async Task<IActionResult> GetAccessTree(CancellationToken cancellationToken)
        {
            var valueKeys= await _accessTreeValueKeyQueryHandler.Handle(cancellationToken);
            return Ok(valueKeys);
        }
    }
}
