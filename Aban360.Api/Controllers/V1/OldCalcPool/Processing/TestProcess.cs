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
        private readonly IBedBesCreateHadler _bedBesCreateHadler;
        public TestProcess(
            IProcessing processing,
            IBedBesCreateHadler bedBesCreateHadler)
        {
            _processing = processing;
            _processing.NotNull(nameof(processing));

            _bedBesCreateHadler = bedBesCreateHadler;
            _bedBesCreateHadler.NotNull(nameof(bedBesCreateHadler));
        }

        [HttpPost, HttpGet]
        [Route("test-by-current-data")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProcessDetailOutputDto>),StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Test(MeterInfoInputDto input,CancellationToken cancellationToken)
        {
            ProcessDetailOutputDto result = await _processing.Handle(input, cancellationToken);
            return Ok(result);
        }
        
        
        
        [HttpPost, HttpGet]
        [Route("test-by-previous-data")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProcessDetailOutputDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> TestByPreviousData(MeterInfoByPreviousDataInputDto input,CancellationToken cancellationToken)
        {
            ProcessDetailOutputDto result = await _processing.Handle(input, cancellationToken);
           //await _bedBesCreateHadler.Handle(result,12, cancellationToken);
            return Ok(result);
        }
        
        
        [HttpPost, HttpGet]
        [Route("test-imaginary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProcessDetailOutputDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> TestImaginary(BaseOldTariffEngineImaginaryInputDto input,CancellationToken cancellationToken)
        {
            ProcessDetailOutputDto result = await _processing.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
