using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;
using DNTPersianUtils.Core;
using System;

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

            ICollection<InvoiceInstallment> invoiceInstallments = new List<InvoiceInstallment>(createDto.InstallmentCount);
            for (int i = 0; i < createDto.InstallmentCount; i++)
            {
                Random random = new Random();

                var invoiceInstallment = new InvoiceInstallment()
                {
                    Invoice = invoice,
                    BillId = createDto.BillId,
                    PaymentId =createDto.PaymentId,
                    InstallmentOrder = i + 1,
                    DueDateJalali = DateTime.Now.AddDays(createDto.PaymentPeriod * i).ToShortPersianDateString(),
                    DueDateTime = DateTime.Now.AddDays(createDto.PaymentPeriod * i),
                };
                if (i == 0)
                {
                    invoiceInstallment.Amount = (long)Math.Ceiling((invoiceLineItem.Sum(i => i.Amount) * createDto.PrepaymentPercent) / 100m);
                }
                else
                {
                    var eachPayment = (invoiceLineItem.Sum(i => i.Amount)) - (invoiceInstallments.Where(i => i.InstallmentOrder == 1).First().Amount);
                    invoiceInstallment.Amount =(long)Math.Ceiling( eachPayment / (createDto.InstallmentCount - 1m));
                }
                invoiceInstallments.Add(invoiceInstallment);
            }


            await _invoiceCommandService.Add(invoice);
            await _invoiceLineItemCommandService.Add(invoiceLineItem);
            await _invoiceInstallmentCommandService.Add(invoiceInstallments);
        }
    }
}
