using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.GatewayAdhoc.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.GatewayAdhoc.Features.Bill.Queries.Implementations
{
    internal sealed class InvoiceInstallmentGetByPaymentIdAddhoc : IInvoiceInstallmentGetByPaymentIdAddhoc
    {
        private readonly IInvoiceInstallmentByPaymentIdGetSingleHandler _invoiceInstallmentGetSingleHandler;
        public InvoiceInstallmentGetByPaymentIdAddhoc(IInvoiceInstallmentByPaymentIdGetSingleHandler invoiceInstallmentGetSingleHandler)
        {
            _invoiceInstallmentGetSingleHandler = invoiceInstallmentGetSingleHandler;
            _invoiceInstallmentGetSingleHandler.NotNull(nameof(invoiceInstallmentGetSingleHandler));
        }

        public async Task<InvoiceInstallment> Handle(string paymentId, CancellationToken cancellationToken)
        {
            var invoiceInstallment = await _invoiceInstallmentGetSingleHandler.Handle(paymentId, cancellationToken);
            return invoiceInstallment;
        }
    }
}
