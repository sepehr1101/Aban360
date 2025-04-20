using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class InvoiceInserterHandler : IInvoiceInserterHandler
    {
        //private readonly IInvoiceCommandService _invoiceCommand;
        //private readonly IInvoiceLineItemCommandService _invoiceLineItemCommand;
        //private readonly IInvoiceInstallmentCommandService _invoiceInstallmentCommand;
        //public InvoiceInserterHandler(
        //    IInvoiceCommandService invoiceCommand,
        //    IInvoiceLineItemCommandService invoiceLineItemCommand,
        //    IInvoiceInstallmentCommandService invoiceInstallmentCommand)
        //{
        //    _invoiceCommand = invoiceCommand;
        //    _invoiceCommand.NotNull(nameof(invoiceCommand));

        //    _invoiceLineItemCommand = invoiceLineItemCommand;
        //    _invoiceLineItemCommand.NotNull(nameof(invoiceLineItemCommand));

        //    _invoiceInstallmentCommand = invoiceInstallmentCommand;
        //    _invoiceInstallmentCommand.NotNull(nameof(invoiceInstallmentCommand));
        //}
        public void Handle(IntervalCalculationResultWrapper intervalCalculationResult, CancellationToken cancellationToken)
        {
            ICollection<IntervalCalculationResult2> Result2 = new List<IntervalCalculationResult2>();
            foreach (var item1 in intervalCalculationResult.IntervalCalculationResults)
            {
                item1.CalculationInfo.ForEach(x => Result2.Add(x));
            }

            var ss = Result2
                 .GroupBy(x => new { x.OfferingTitle })
                 .Select(y => new invoiceLineItemList()
                 {
                     OfferingTitle = y.Key.OfferingTitle,
                     invoiceLineItem = y.Select(z => new invoiceLineItemResult()
                     {
                         Amount = z.Amount,
                         Consumption = z.Consumption,
                         Duration = z.Duration,
                         Formula = z.Formula,
                         LineItemTypeTitle = z.LineItemTypeTitle
                     }).ToList()
                 }).ToList();
            // ta inja Offering be dast omad
            //bayad baraye har offering jam Amout hesab konam
            //va insert konam



            Invoice invoice = new Invoice()
            {
                InvoiceTypeId = 1,//Todo
                InvoiceStatusId = 1,//Todo
                Amount = (long)intervalCalculationResult.Amount,
                OfferingCount = (short)intervalCalculationResult.IntervalCount,
                DepositRate = 100,
                InstallmentCount = 1,
            };
            // await _invoiceCommand.Add(invoice);
            //InvoiceLineItem invoiceLineItem = new InvoiceLineItem()
            //{
            //    Invoice=invoice,
            //    OfferingId=,
            //    InvoinceLineItemInsertModeId=,
            //    Amount=,
            //    Quanity=
            //};
        }

    }
}
