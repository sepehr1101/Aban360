using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/old-calc")]
    public class TestProcess:BaseController
    {
        private readonly IProcessing _processing;
        public TestProcess(IProcessing processing)
        {
            _processing = processing;
            _processing.NotNull(nameof(processing));
        }

        [HttpPost, HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test(MeterInfoInputDto input,CancellationToken cancellationToken)
        {
            await _processing.Handle(input, cancellationToken);
            return Ok();
        }
    }
}
