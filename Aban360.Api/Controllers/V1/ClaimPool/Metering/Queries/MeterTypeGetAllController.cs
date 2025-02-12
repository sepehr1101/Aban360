﻿using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("meter-type")]
    public class MeterTypeGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterTypeGetAllHandler _meterTypeHandler;
        public MeterTypeGetAllController(
            IUnitOfWork uow,
            IMeterTypeGetAllHandler meterTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterTypeHandler = meterTypeHandler;
            _meterTypeHandler.NotNull(nameof(meterTypeHandler));
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var meterType = await _meterTypeHandler.Handle(cancellationToken);
            return Ok(meterType);
        }
    }
}
