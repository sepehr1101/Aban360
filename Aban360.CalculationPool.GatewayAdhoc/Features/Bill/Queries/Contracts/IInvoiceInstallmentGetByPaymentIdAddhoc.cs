using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.GatewayAdhoc.Features.Bill.Queries.Contracts
{
    public interface IInvoiceInstallmentGetByPaymentIdAddhoc
    {
        Task<InvoiceInstallment> Handle(string paymentId, CancellationToken cancellationToken);
    }
}
