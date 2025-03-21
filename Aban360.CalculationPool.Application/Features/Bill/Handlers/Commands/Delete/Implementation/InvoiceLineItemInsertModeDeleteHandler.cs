﻿using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Implementation
{
    internal sealed class InvoiceLineItemInsertModeDeleteHandler : IInvoiceLineItemInsertModeDeleteHandler
    {
        private readonly IInvoiceLineItemInsertModeCommandService _invoiceLineItemInsertModeCommandService;
        private readonly IInvoiceLineItemInsertModeQueryService _invoiceLineItemInsertModeQueryService;
        public InvoiceLineItemInsertModeDeleteHandler(
            IInvoiceLineItemInsertModeCommandService invoiceLineItemInsertModeCommandService,
            IInvoiceLineItemInsertModeQueryService invoiceLineItemInsertModeQueryService)
        {
            _invoiceLineItemInsertModeCommandService = invoiceLineItemInsertModeCommandService;
            _invoiceLineItemInsertModeCommandService.NotNull(nameof(invoiceLineItemInsertModeCommandService));

            _invoiceLineItemInsertModeQueryService = invoiceLineItemInsertModeQueryService;
            _invoiceLineItemInsertModeQueryService.NotNull(nameof(invoiceLineItemInsertModeQueryService));
        }

        public async Task Handle(InvoiceLineItemInsertModeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            InvoiceLineItemInsertMode invoiceLineItemInsertMode = await _invoiceLineItemInsertModeQueryService.Get(deleteDto.Id);
            await _invoiceLineItemInsertModeCommandService.Remove(invoiceLineItemInsertMode);
        }
    }
}
