using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.Loging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Loging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Loging.Dto.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.Loging.Queries
{
    [Route("v1/logging")]
    public class LoggingGetByDateTimeController : BaseController
    {
        private readonly ILoggingGetyDateTimeHandler _loggingHandler;
        public LoggingGetByDateTimeController(ILoggingGetyDateTimeHandler logingHandler)
        {
            _loggingHandler = logingHandler;
            _loggingHandler.NotNull(nameof(_loggingHandler));
        }

        [Route("get")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<LoggingOutputDto>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(LoggingInputByStringDto inputDto, CancellationToken cancellation)
        {
            IEnumerable<LoggingOutputDto> result = await _loggingHandler.Handle(inputDto, cancellation);
            return Ok(result);
        }
    }
}
