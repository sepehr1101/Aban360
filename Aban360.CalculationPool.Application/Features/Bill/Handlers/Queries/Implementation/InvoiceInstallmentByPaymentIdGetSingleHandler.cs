using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class InvoiceInstallmentByPaymentIdGetSingleHandler : IInvoiceInstallmentByPaymentIdGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceInstallmentQueryService _invoiceInstallmentQueryService;
        public InvoiceInstallmentByPaymentIdGetSingleHandler(
            IMapper mapper,
            IInvoiceInstallmentQueryService invoiceInstallmentQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceInstallmentQueryService = invoiceInstallmentQueryService;
            _invoiceInstallmentQueryService.NotNull(nameof(invoiceInstallmentQueryService));
        }

        public async Task<InvoiceInstallment> Handle(string paymentId,CancellationToken cancellationToken)
        {
            InvoiceInstallment invoiceInstallment = await _invoiceInstallmentQueryService.Get(paymentId);
            //return _mapper.Map<InvoiceInstallmentGetDto>(invoiceInstallment);
            return invoiceInstallment;
        }
    }
}
