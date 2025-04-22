using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class InvoiceInserterHandler : IInvoiceInserterHandler
    {
        private readonly IInvoiceCommandService _invoiceCommand;
        private readonly IInvoiceLineItemCommandService _invoiceLineItemCommand;
        private readonly IInvoiceInstallmentCommandService _invoiceInstallmentCommand;
        private readonly IOfferingQueryService _offeringQueryService;
        private readonly IWaterMeterChangeNumberHistoryCommandService _waterMeterChangeNumberHistoryCommand;    
        public InvoiceInserterHandler(
            IInvoiceCommandService invoiceCommand,
            IInvoiceLineItemCommandService invoiceLineItemCommand,
            IInvoiceInstallmentCommandService invoiceInstallmentCommand,
            IOfferingQueryService offeringQueryService,
            IWaterMeterChangeNumberHistoryCommandService waterMeterChangeNumberHistoryCommand)
        {
            _invoiceCommand = invoiceCommand;
            _invoiceCommand.NotNull(nameof(invoiceCommand));

            _invoiceLineItemCommand = invoiceLineItemCommand;
            _invoiceLineItemCommand.NotNull(nameof(invoiceLineItemCommand));

            _invoiceInstallmentCommand = invoiceInstallmentCommand;
            _invoiceInstallmentCommand.NotNull(nameof(invoiceInstallmentCommand));

            _offeringQueryService = offeringQueryService;
            _offeringQueryService.NotNull(nameof(offeringQueryService));

            _waterMeterChangeNumberHistoryCommand = waterMeterChangeNumberHistoryCommand;
            _waterMeterChangeNumberHistoryCommand.NotNull(nameof(waterMeterChangeNumberHistoryCommand));
        }
        public async Task Handle(IntervalCalculationResultWrapper intervalCalculationResult, CancellationToken cancellationToken)
        {
            ICollection<IntervalCalculationResult2> allIntervalCalculationResult2 = new List<IntervalCalculationResult2>();
            foreach (var result3 in intervalCalculationResult.IntervalCalculationResults)
            {
                result3.CalculationInfo.ForEach(result2 => allIntervalCalculationResult2.Add(result2));
            }

            var invoiceLineItemList = allIntervalCalculationResult2
                 .GroupBy(intervalResult2 => new { intervalResult2.OfferingTitle })
                 .Select(result2 => new invoiceLineItemList()
                 {
                     OfferingTitle = result2.Key.OfferingTitle,
                     invoiceLineItem = result2.Select(result => new invoiceLineItemResult()
                     {
                         Amount = (long)result.Amount,
                         Consumption = result.Consumption,
                         Duration = result.Duration,
                         Formula = result.Formula,
                         LineItemTypeTitle = result.LineItemTypeTitle
                     }).ToList()
                 }).ToList();

            var offerings = await _offeringQueryService.Get();


            Invoice invoice = new Invoice()
            {
                InvoiceTypeId = 1,//Todo
                InvoiceStatusId = 1,//Todo
                Amount = (long)intervalCalculationResult.Amount,
                OfferingCount = (short)invoiceLineItemList.Count,
                DepositRate = 100,
                InstallmentCount = 1,
            };
            ICollection<InvoiceLineItem> invoiceLineItems = invoiceLineItemList.Select(itemList =>
            {
                var offering = offerings.Single(offer => offer.Title == itemList.OfferingTitle);
                var offeringAmount = itemList.invoiceLineItem.Sum(itemResult => itemResult.Amount);

                return new InvoiceLineItem()
                {
                    Invoice = invoice,
                    OfferingId = offering.Id,
                    InvoiceLineItemInsertModeId= InvoiceLineItemInsertModeEnum.BySystem,//Todo
                    Amount = offeringAmount,
                    Quanity = 1//Todo
                };
            }).ToList();
            InvoiceInstallment invoiceInstallment = new InvoiceInstallment()
            {
                Invoice=invoice,
                Amount=invoice.Amount,
                DueDateJalali=DateTime.Now.ToShortPersianDateString(),
                DueDateTime=DateTime.Now,
                InstallmentOrder=1,//Todo
                BillId="102030",//dont have BillId
                PaymentId="102030"//Todo
            };

            ///WaterMeterChangeNumberHistory
            WaterMeterChangeNumberHistory waterMeterHistory = new WaterMeterChangeNumberHistory()
            {
                Consumption=(long) intervalCalculationResult.Consumption,
                ConstumptionAverage=(long)intervalCalculationResult.ConsumptionAverage,//Todo : change Date Type
                ChangeMeterReasonId=1,//Todo
                InvoiceInstallment= invoiceInstallment
            };

            await _invoiceCommand.Add(invoice);
            await _invoiceLineItemCommand.Add(invoiceLineItems);
            await _invoiceInstallmentCommand.Add(invoiceInstallment);
            await _waterMeterChangeNumberHistoryCommand.Add(waterMeterHistory);
            //return "finish";
        }

    }

    public record invoiceLineItemList
    {
        public ICollection<invoiceLineItemResult> invoiceLineItem { get; set; }
        public string OfferingTitle { get; set; } = default!;

    }
    public record invoiceLineItemResult
    {
        public string Formula { get; set; } = default!;
        public double Consumption { get; set; }
        public int Duration { get; set; }
        public long Amount { get; set; }
        public string LineItemTypeTitle { get; set; } = default!;

    }
}
