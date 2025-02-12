﻿using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("meter-producer")]
    public class MeterProducerCreateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterProducerCreateHandler _meterProducerHandler;
        public MeterProducerCreateController(
            IUnitOfWork uow, 
            IMeterProducerCreateHandler meterProducerHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterProducerHandler = meterProducerHandler;
            _meterProducerHandler.NotNull(nameof(meterProducerHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] MeterProducerCreateDto createDto, CancellationToken cancellationToken)
        {
            await _meterProducerHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
