﻿using Aban360.CalculationPool.Application.Features.TestTariff.Handlers.Contrats;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/tariff-calculation")]
    public class TariffBatchCalculationManager : BaseController
    {
        private readonly ITariffTestBatchHandler _tariffCalculationHandler;
        public TariffBatchCalculationManager(
            ITariffTestBatchHandler tariffCalculationHandler)
        {
            _tariffCalculationHandler = tariffCalculationHandler;
            _tariffCalculationHandler.NotNull();
        }

        [HttpPost]
        [Route("test-batch")]
        [AllowAnonymous]
        public async Task<IActionResult> Test([FromBody] CaluclationIntervalBatchTestInput tariffTestInput, CancellationToken cancellationToken)
        {
            CaluclationIntervalDiscrepancytWrapper wrapper = await _tariffCalculationHandler.Handle(tariffTestInput, cancellationToken);
            return Ok(wrapper);
        }
    }
}
