﻿using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    internal sealed class InvoiceTypeUpdateHandler : IInvoiceTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceTypeQueryService _invoiceTypeQueryService;
        public InvoiceTypeUpdateHandler(
            IMapper mapper,
            IInvoiceTypeQueryService invoiceTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceTypeQueryService = invoiceTypeQueryService;
            _invoiceTypeQueryService.NotNull(nameof(invoiceTypeQueryService));
        }

        public async Task Handle(InvoiceTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            InvoiceType invoiceType = await _invoiceTypeQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, invoiceType);
        }
    }
}
