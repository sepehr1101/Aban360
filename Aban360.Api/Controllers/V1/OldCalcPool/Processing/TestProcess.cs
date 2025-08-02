using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Authorization;
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
        [Route("test-by-current-data")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterInfoInputDto>),StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Test(MeterInfoInputDto input,CancellationToken cancellationToken)
        {
            ProcessDetailOutputDto result = await _processing.Handle(input, cancellationToken);
            return Ok(result);
        }
        
        
        
        [HttpPost, HttpGet]
        [Route("test-by-previous-data")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterInfoByPreviousDataInputDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> TestByPreviousData(MeterInfoByPreviousDataInputDto input,CancellationToken cancellationToken)
        {
            ProcessDetailOutputDto result = await _processing.Handle(input, cancellationToken);
            return Ok(result);
        }
        
        
        [HttpPost, HttpGet]
        [Route("test-imaginary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BaseOldTariffEngineImaginaryInputDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> TestImaginary(BaseOldTariffEngineImaginaryInputDto input,CancellationToken cancellationToken)
        {
            ProcessDetailOutputDto result = await _processing.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
