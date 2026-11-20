using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Services;
public interface IInvoiceService
{
    Task InvoiceCreateAsync(InvoiceCreateDto createDto, CancellationToken cancellationToken);
    Task InvoiceInserterAsync(IntervalCalculationResultWrapper intervalCalculationResult, CancellationToken cancellationToken);
}

internal sealed class InvoiceService : IInvoiceService
{
    private readonly IMapper _mapper;
    private readonly IInvoiceCommandService _invoiceCommandService;
    private readonly IValidator<InvoiceCreateDto> _validator;
    private readonly IInvoiceCommandService _invoiceCommand;
    private readonly IInvoiceLineItemCommandService _invoiceLineItemCommand;
    private readonly IInvoiceInstallmentCommandService _invoiceInstallmentCommand;
    private readonly IOfferingQueryService _offeringQueryService;
    private readonly IWaterMeterChangeNumberHistoryCommandService _waterMeterChangeNumberHistoryCommand;
    public InvoiceService(IMapper mapper, IInvoiceCommandService invoiceCommandService, IValidator<InvoiceCreateDto> validator, IInvoiceLineItemCommandService invoiceLineItemCommand, IInvoiceInstallmentCommandService invoiceInstallmentCommand, IOfferingQueryService offeringQueryService, IWaterMeterChangeNumberHistoryCommandService waterMeterChangeNumberHistoryCommand)
    {
        _mapper = mapper;
        _mapper.NotNull(nameof(mapper));
        _invoiceCommandService = invoiceCommandService;
        _invoiceCommandService.NotNull(nameof(invoiceCommandService));
        _validator = validator;
        _validator.NotNull(nameof(validator));
        _invoiceCommand = invoiceCommandService;
        _invoiceCommand.NotNull(nameof(invoiceCommandService));
        _invoiceLineItemCommand = invoiceLineItemCommand;
        _invoiceLineItemCommand.NotNull(nameof(invoiceLineItemCommand));
        _invoiceInstallmentCommand = invoiceInstallmentCommand;
        _invoiceInstallmentCommand.NotNull(nameof(invoiceInstallmentCommand));
        _offeringQueryService = offeringQueryService;
        _offeringQueryService.NotNull(nameof(offeringQueryService));
        _waterMeterChangeNumberHistoryCommand = waterMeterChangeNumberHistoryCommand;
        _waterMeterChangeNumberHistoryCommand.NotNull(nameof(waterMeterChangeNumberHistoryCommand));
    }

    public async Task InvoiceCreateAsync(InvoiceCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }

        Invoice invoice = _mapper.Map<Invoice>(createDto);
        await _invoiceCommandService.Add(invoice);
    }

    public async Task InvoiceInserterAsync(IntervalCalculationResultWrapper intervalCalculationResult, CancellationToken cancellationToken)
    {
        ICollection<IntervalCalculationResult2> allIntervalCalculationResult2 = new List<IntervalCalculationResult2>();
        foreach (var result3 in intervalCalculationResult.IntervalCalculationResults)
        {
            result3.CalculationInfo.ForEach(result2 => allIntervalCalculationResult2.Add(result2));
        }

        var invoiceLineItemList = allIntervalCalculationResult2.GroupBy(intervalResult2 => new { intervalResult2.OfferingTitle }).Select(result2 => new invoiceLineItemList() { OfferingTitle = result2.Key.OfferingTitle, invoiceLineItem = result2.Select(result => new invoiceLineItemResult() { Amount = (long)result.Amount, Consumption = result.Consumption, Duration = result.Duration, Formula = result.Formula, LineItemTypeTitle = result.LineItemTypeTitle }).ToList() }).ToList();
        var offerings = await _offeringQueryService.Get();
        Invoice invoice = new Invoice()
        {
            InvoiceTypeId = 1, //Todo
            InvoiceStatusId = 1, //Todo
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
                InvoiceLineItemInsertModeId = InvoiceLineItemInsertModeEnum.BySystem, //Todo
                Amount = offeringAmount,
                Quanity = 1 //Todo
            };
        }).ToList();
        InvoiceInstallment invoiceInstallment = new InvoiceInstallment()
        {
            Invoice = invoice,
            Amount = invoice.Amount,
            DueDateJalali = DateTime.Now.ToShortPersianDateString(),
            DueDateTime = DateTime.Now,
            InstallmentOrder = 1, //Todo
            BillId = "102030", //dont have BillId
            PaymentId = "102030" //Todo
        };
        ///WaterMeterChangeNumberHistory
        WaterMeterChangeNumberHistory waterMeterHistory = new WaterMeterChangeNumberHistory()
        {
            Consumption = intervalCalculationResult.Consumption,
            ConstumptionAverage = (float)intervalCalculationResult.ConsumptionAverage,
            ChangeMeterReasonId = 1, //Todo
            Invoice = invoice
        };
        await _invoiceCommand.Add(invoice);
        await _invoiceLineItemCommand.Add(invoiceLineItems);
        await _invoiceInstallmentCommand.Add(invoiceInstallment);
        await _waterMeterChangeNumberHistoryCommand.Add(waterMeterHistory);
    }
}
