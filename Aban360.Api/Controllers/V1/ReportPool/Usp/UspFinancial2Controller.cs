using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Usp.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Usp.Input;
using Aban360.ReportPool.Domain.Features.Usp.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Usp
{
    [Route("v1/usp")]
    public class UspFinancial2Controller:BaseController
    {
        private readonly IUspFinancial2Handler _handler;
        public UspFinancial2Controller(IUspFinancial2Handler handler)
        {
            _handler = handler;
            _handler.NotNull(nameof(handler));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("financial2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UspFinancial2Output>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] UspFinancial2Input input, CancellationToken cancellationToken)
        {
            input.VillageOrCityType = input.ZoneId > 140000 ? 2 : 1;
            IEnumerable<UspFinancial2Output> output = await _handler.Handle(input, cancellationToken);
            return Ok(output);
        }
    }
}
