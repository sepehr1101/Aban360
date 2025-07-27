using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.Loging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Application.Features.ServerInfo.Handlers.Contracts;
using Aban360.SystemPool.Domain.Features.Loging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Loging.Dto.Output;
using Aban360.SystemPool.Domain.Features.ServerInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.Loging.Queries
{
    [Route("v1/loging")]
    public class LogingGetByDateTimeController : BaseController
    {
        private readonly ILogingGetyDateTimeHandler _logingHandler;
        public LogingGetByDateTimeController(ILogingGetyDateTimeHandler logingHandler)
        {
            _logingHandler = logingHandler;
            _logingHandler.NotNull(nameof(_logingHandler));
        }

        [Route("get")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<LogingOutputDto>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Get(LogingInputByStringDto inputDto, CancellationToken cancellation)
        {
            IEnumerable<LogingOutputDto> result = await _logingHandler.Handle(inputDto, cancellation);
            return Ok(result);
        }
    }
}
