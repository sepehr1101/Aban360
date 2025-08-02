using Aban360.CalculationPool.Application.Features.Base;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Constant;
using Aban360.OldCalcPool.Application.Exceptions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using DNTPersianUtils.Core;
using NetTopologySuite.Index.HPRtree;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class Processing : BaseOldTariffEngine, IProcessing
    {
        private readonly ICustomerInfoDetailQueryService _customerInfoDetailQueryService;
        private readonly IMeterInfoDetailQueryService _meterInfoDetailQueryService;
        private readonly INerkhGetByConsumptionService _nerkhGetByConsumptionService;
        int thresholdDay = 4;
        public Processing(
            ICustomerInfoDetailQueryService customerInfoDetailQueryService,
            IMeterInfoDetailQueryService meterInfoDetailQueryService,
            INerkhGetByConsumptionService nerkhGetByConsumptionService)
        {
            _customerInfoDetailQueryService = customerInfoDetailQueryService;
            _customerInfoDetailQueryService.NotNull(nameof(customerInfoDetailQueryService));

            _meterInfoDetailQueryService = meterInfoDetailQueryService;
            _meterInfoDetailQueryService.NotNull(nameof(meterInfoDetailQueryService));

            _nerkhGetByConsumptionService = nerkhGetByConsumptionService;
            _nerkhGetByConsumptionService.NotNull(nameof(nerkhGetByConsumptionService));
        }

        public async Task Handle(ConsumptionInputDto input, CancellationToken cancellationToken)
        {
            int consumption = GetConsumption(input.PreviousMeterNumber, input.CurrentMeterNumber);
            int duration = GetDuration(input.PreviousDateJalali, input.CurrentDateJalali);
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration,1);
        }
        private int GetConsumption(int previousNumber, int currentNumber)
        {
            return currentNumber - previousNumber;
        }
        private int GetDuration(string previousDate, string currentDate)
        {
            var previousGregorian = previousDate.ToGregorianDateTime();
            var currentGregorian = currentDate.ToGregorianDateTime();
            return (currentGregorian.Value - previousGregorian.Value).Days;
        }
        private double GetDailyConsumptionAverage(int masraf, int duration,int domesticUnit)
        {
            int domesticUnitTemp = domesticUnit < 1 ? 1 : domesticUnit;
            return masraf / (double)duration/domesticUnitTemp;
        }
        private void Validation(string previousDateJalali)
        {
            DateOnly? previousDate = previousDateJalali.ToGregorianDateOnly();
            if (!previousDate.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidDate);
            }
            if (previousDate.Value > DateOnly.FromDateTime(DateTime.Now.AddDays(-thresholdDay)))
            {
                throw new BaseException(ExceptionLiterals.InvalidPreviousDateInvoice(thresholdDay));
            }
        }
        //private DateOnly ConvertJalaliToGregorian(string dateJalali)
        //{
        //    DateOnly? grogorianDate = dateJalali.ToGregorianDateOnly();
        //    if (!grogorianDate.HasValue)
        //    {
        //        throw new BaseException(ExceptionLiterals.InvalidDate);
        //    }

        //    return grogorianDate.Value;
        //}
        //private (int, double) CalcPartial(NerkhGetDto nerkh, DateOnly previousDate, DateOnly currentDate, double dailyAverage)
        //{
        //    DateOnly fromDate = ConvertJalaliToGregorian(nerkh.Date1);
        //    DateOnly toDate = ConvertJalaliToGregorian(nerkh.Date2);

        //    DateOnly startSegment = fromDate > previousDate ? fromDate : previousDate;
        //    DateOnly endSegment = toDate < currentDate ? toDate : currentDate;

        //    int duration = endSegment.DayNumber - startSegment.DayNumber;
        //    double partialConsumption = duration * dailyAverage;
        //    return (duration, partialConsumption);
        //}
        //
        public async Task<ProcessDetailOutputDto> Handle(MeterInfoInputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            MeterInfoOutputDto meterInfo = await _meterInfoDetailQueryService.GetInfo(new CustomerInfoInputDto(customerInfo.ZoneId, customerInfo.Radif));
            Validation(meterInfo.PreviousDateJalali);

            int consumption = GetConsumption(meterInfo.PreviousNumber, input.CurrentMeterNumber);
            int duration = GetDuration(meterInfo.PreviousDateJalali, input.CurrentDateJalali);
            if (duration < 5)
            {
                throw new InvalidBillParametersException(Literals.InvalidDuration);
            }
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, customerInfo.DomesticUnit) ;
            double monthlyAverageConsumption = dailyAverage * 30 ;

            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadGetDto>, IEnumerable<ZaribGetDto>) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.Get(new NerkhByConsumptionInputDto(customerInfo.ZoneId,
                                                                                                                       customerInfo.UsageId,
                                                                                                                       meterInfo.PreviousDateJalali,
                                                                                                                       input.CurrentDateJalali,
                                                                                                                       monthlyAverageConsumption));
            ProcessDetailOutputDto result = Tarif_Ab(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, dailyAverage, meterInfo.PreviousDateJalali, input.CurrentDateJalali, customerInfo, meterInfo);
            result.Customer = customerInfo;
            result.MeterInfo = meterInfo;
            return result;
        }
        public async Task<ProcessDetailOutputDto> Handle(MeterInfoByPreviousDataInputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            Validation(input.PreviousDateJalali);

            int consumption = GetConsumption(input.PreviousNumber, input.CurrentMeterNumber);
            int duration = GetDuration(input.PreviousDateJalali, input.CurrentDateJalali);
            if (duration < 5)
            {
                throw new InvalidBillParametersException(Literals.InvalidDuration);
            }
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration, customerInfo.DomesticUnit) ;
            double monthlyAverageConsumption = dailyAverage * 30 ;

            (IEnumerable<NerkhGetDto>, IEnumerable<AbAzadGetDto>, IEnumerable<ZaribGetDto>) allNerkhAbAbAzad = await _nerkhGetByConsumptionService.Get(new NerkhByConsumptionInputDto(customerInfo.ZoneId,
                                                                                                                       customerInfo.UsageId,
                                                                                                                       input.PreviousDateJalali,
                                                                                                                       input.CurrentDateJalali,
                                                                                                                       monthlyAverageConsumption));
          MeterInfoOutputDto meterInfo = new MeterInfoOutputDto()
          {
              PreviousDateJalali = input.PreviousDateJalali,
              PreviousNumber=input.PreviousNumber,
          };
            ProcessDetailOutputDto result = Tarif_Ab(allNerkhAbAbAzad.Item1, allNerkhAbAbAzad.Item2, allNerkhAbAbAzad.Item3, dailyAverage, input.PreviousDateJalali, input.CurrentDateJalali, customerInfo, meterInfo);
            result.Customer = customerInfo;
            result.MeterInfo = meterInfo;
            return result;
        }

        private ProcessDetailOutputDto Tarif_Ab(IEnumerable<NerkhGetDto> allNerkh, IEnumerable<AbAzadGetDto> abAzad, IEnumerable<ZaribGetDto> zarib, double dailyAverage, string previousDateJalali, string currentDateJalali, CustomerInfoOutputDto customerInfo, MeterInfoOutputDto meterInfo)
        {
            int counter = 0;
            double sumAbBaha = 0;
            foreach (var nerkhItem in allNerkh)
            {
                AbAzadGetDto abAzadItem = abAzad.ElementAt(counter);
                ZaribGetDto zaribItem = zarib.ElementAt(counter);
                double result = CalculateWaterBill(nerkhItem, abAzadItem, zaribItem, customerInfo, meterInfo,dailyAverage,currentDateJalali);
                nerkhItem.CalcVaj = result.ToString();
                sumAbBaha += result;
                counter++;
            }
            

            return new ProcessDetailOutputDto(sumAbBaha,allNerkh,abAzad,zarib);
        }
        
    }
    public record ProcessDetailOutputDto
    {
        public double InvoiceAmount { get; set; }
        public IEnumerable<NerkhGetDto> Nerkh { get; set; }
        public IEnumerable<AbAzadGetDto> AbAzad { get; set; }
        public IEnumerable<ZaribGetDto> Zarib { get; set; }
        public CustomerInfoOutputDto Customer { get; set; }
        public MeterInfoOutputDto  MeterInfo { get; set; }


        public ProcessDetailOutputDto(double _invoiceAmount, IEnumerable<NerkhGetDto> _nerkh, IEnumerable<AbAzadGetDto> _abAzad, IEnumerable<ZaribGetDto> _zarib)
        {
            InvoiceAmount = _invoiceAmount;
            Nerkh = _nerkh;
            AbAzad = _abAzad;
            Zarib = _zarib;
        }
    }
}