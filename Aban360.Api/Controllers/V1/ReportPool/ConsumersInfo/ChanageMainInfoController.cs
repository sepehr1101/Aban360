using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/change-main")]
    public class ChangeMainInfoController : BaseController
    {
        private readonly IChangeMainInfoGetHandler _changeMainInfoGetHandler;
        public ChangeMainInfoController(IChangeMainInfoGetHandler changeMainInfoGetHandler)
        {
            _changeMainInfoGetHandler = changeMainInfoGetHandler;
            _changeMainInfoGetHandler.NotNull(nameof(changeMainInfoGetHandler));
        }

        [HttpPost]
        [Route("info")]//todo: change url
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ChangeMainOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Info([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            ICollection<ChangeMainOutputDto> summary = await _changeMainInfoGetHandler.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }
    }
}
