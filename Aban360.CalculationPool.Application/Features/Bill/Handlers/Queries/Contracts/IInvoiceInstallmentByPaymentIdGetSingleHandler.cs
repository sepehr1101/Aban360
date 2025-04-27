using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts
{
    public interface IInvoiceInstallmentByPaymentIdGetSingleHandler
    {
        Task<InvoiceInstallment> Handle(string billId,CancellationToken cancellationToken);
    }
}
