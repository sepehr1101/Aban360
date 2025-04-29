using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;
using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class InvoiceTogetherCreateHandler : IInvoiceTogetherCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceCommandService _invoiceCommandService;
        private readonly IInvoiceLineItemCommandService _invoiceLineItemCommandService;
        private readonly IInvoiceInstallmentCommandService _invoiceInstallmentCommandService;
        public InvoiceTogetherCreateHandler(
            IMapper mapper,
            IInvoiceCommandService invoiceCommandService,
            IInvoiceLineItemCommandService invoiceLineItemCommandService,
            IInvoiceInstallmentCommandService invoiceInstallmentCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _invoiceCommandService = invoiceCommandService;
            _invoiceCommandService.NotNull(nameof(invoiceCommandService));

            _invoiceLineItemCommandService = invoiceLineItemCommandService;
            _invoiceLineItemCommandService.NotNull(nameof(invoiceLineItemCommandService));

            _invoiceInstallmentCommandService = invoiceInstallmentCommandService;
            _invoiceInstallmentCommandService.NotNull(nameof(invoiceInstallmentCommandService));
        }

        public async Task Handle(InvoiceTogetherCreateDto createDto, CancellationToken cancellationToken)
        {
            Invoice invoice = _mapper.Map<Invoice>(createDto.Invoice);
            invoice.Amount = createDto.InvoiceLineItem.Sum(i => i.Amount);
            invoice.OfferingCount = (short)createDto.InvoiceLineItem.Count();

            ICollection<InvoiceLineItem> invoiceLineItem = _mapper.Map<ICollection<InvoiceLineItem>>(createDto.InvoiceLineItem);
            invoiceLineItem.ForEach(i =>
            {
                i.Invoice = invoice;
                i.InvoiceLineItemInsertModeId = InvoiceLineItemInsertModeEnum.ByUser;
            });

            InvoiceInstallment invoiceInstallment = new InvoiceInstallment()
            {
                Invoice = invoice,
                Amount = invoiceLineItem.Sum(i => i.Amount),
                DueDateJalali = DateTime.Now.ToShortPersianDateString(),
                DueDateTime = DateTime.Now,//Todo
                InstallmentOrder = 1,
                BillId = createDto.BillId,
                PaymentId = createDto.PaymentId,
            };


            await _invoiceCommandService.Add(invoice);
            await _invoiceLineItemCommandService.Add(invoiceLineItem);
            await _invoiceInstallmentCommandService.Add(invoiceInstallment);
        }
    }
}
