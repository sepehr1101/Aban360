using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/old-calc-new")]
    public class TestTariffCalculationController : BaseController
    {
        private readonly IOldTariffEngine _oldTariffEngine;
        private readonly IBedBesCreateHadler _bedBesCreateHadler;
        public TestTariffCalculationController(
            IOldTariffEngine processing,
            IBedBesCreateHadler bedBesCreateHadler)
        {
            _oldTariffEngine = processing;
            _oldTariffEngine.NotNull(nameof(processing));

            _bedBesCreateHadler = bedBesCreateHadler;
            _bedBesCreateHadler.NotNull(nameof(bedBesCreateHadler));
        }

        [HttpPost, HttpGet]
        [Route("test-by-current-data")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbBahaCalculationDetails>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Test(MeterInfoInputDto input, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails result = await _oldTariffEngine.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("test-by-previous-data")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbBahaCalculationDetails>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> TestByPreviousData(MeterInfoByPreviousDataInputDto input, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails result = await _oldTariffEngine.Handle(input, cancellationToken);           
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("test-imaginary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbBahaCalculationDetails>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> TestImaginary(MeterImaginaryInputDto input, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails result = await _oldTariffEngine.Handle(input, cancellationToken);
            return Ok(result);
        }
        
        
        [HttpPost, HttpGet]
        [Route("by-average")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbBahaCalculationDetails>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> ByAverage(MeterDateInfoWithMonthlyConsumptionOutputDto input, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails result = await _oldTariffEngine.Handle(input, cancellationToken);
            return Ok(result);
        }
        
        
        [HttpPost, HttpGet]
        [Route("by-previous-average")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbBahaCalculationDetails>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> ByPreviousAverage(MeterDateInfoByLastMonthlyConsumptionOutputDto input, CancellationToken cancellationToken)
        {
            AbBahaCalculationDetails result = await _oldTariffEngine.Handle(input, cancellationToken);
            return Ok(result);
        }
    }
}
