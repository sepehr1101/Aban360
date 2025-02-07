using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/captcha")]
    public class CaptchaDictionaryController : BaseController
    {
        private readonly ICaptchaDictionaryHandler _captchaDictionaryHandler;
        public CaptchaDictionaryController(ICaptchaDictionaryHandler captchaDictionaryHandler)
        {
            _captchaDictionaryHandler = captchaDictionaryHandler;
            _captchaDictionaryHandler.NotNull(nameof(captchaDictionaryHandler));
        }

        [HttpGet]
        [Route("dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDictionary(CancellationToken cancellationToken)
        {
            var dictionary= await _captchaDictionaryHandler.Handle(cancellationToken);
            return Ok(dictionary);
        }
    }
}