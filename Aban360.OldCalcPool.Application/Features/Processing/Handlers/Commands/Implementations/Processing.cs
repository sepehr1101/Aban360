using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
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
    internal sealed class Processing : IProcessing
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

        public async Task Handle(ConsumptionInputDto intput, CancellationToken cancellationToken)
        {
            int consumption = GetConsumption(intput.PreviousMeterNumber, intput.CurrentMeterNumber);
            int duration = GetDuration(intput.PreviousDateJalali, intput.CurrentDateJalali);
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration);
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
        private double GetDailyConsumptionAverage(int masraf, int duration)
        {
            return masraf / (double)duration;
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
        private DateOnly ConvertJalaliToGregorian(string dateJalali)
        {
            DateOnly? grogorianDate = dateJalali.ToGregorianDateOnly();
            if (!grogorianDate.HasValue)
            {
                throw new BaseException(ExceptionLiterals.InvalidDate);
            }

            return grogorianDate.Value;
        }
        private double CalcPartial(NerkhGetDto nerkh, DateOnly previousDate, DateOnly currentDate, double dailyAverage)
        {
            DateOnly fromDate = ConvertJalaliToGregorian(nerkh.Date1);
            DateOnly toDate = ConvertJalaliToGregorian(nerkh.Date2);

            DateOnly startSegment = fromDate > previousDate ? fromDate : previousDate;
            DateOnly endSegment = toDate < currentDate ? toDate : currentDate;

            int duration = endSegment.DayNumber - startSegment.DayNumber;
            double partialConsumption = duration * dailyAverage;
            return partialConsumption;
        }
        //
        public async Task Handle(MeterInfoInputDto input, CancellationToken cancellationToken)
        {
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            MeterInfoOutputDto meterInfo = await _meterInfoDetailQueryService.GetInfo(new CustomerInfoInputDto(customerInfo.ZoneId, customerInfo.Radif));
            Validation(meterInfo.PreviousDateJalali);

            int consumption = GetConsumption(meterInfo.PreviousNumber, input.CurrentMeterNumber);
            int duration = GetDuration(meterInfo.PreviousDateJalali, input.CurrentDateJalali);
            double dailyAverage = GetDailyConsumptionAverage(consumption, duration);

            int domesticUnitTemp = customerInfo.DomesticUnit < 1 ? 1 : customerInfo.DomesticUnit;
            double monthlyAverageConsumption = dailyAverage * 30 / domesticUnitTemp;

            IEnumerable<NerkhGetDto> allNerkh = await _nerkhGetByConsumptionService.Get(new NerkhByConsumptionInputDto(customerInfo.ZoneId,
                                                                                                                       customerInfo.UsageId,
                                                                                                                       meterInfo.PreviousDateJalali,
                                                                                                                       input.CurrentDateJalali,
                                                                                                                       monthlyAverageConsumption));
            Tarif_Ab(allNerkh, dailyAverage, meterInfo.PreviousDateJalali, input.CurrentDateJalali);

        }

        private void Tarif_Ab(IEnumerable<NerkhGetDto> allNerkh, double dailyAverage, string previousDateJalali, string currentDateJalali)
        {
            DateOnly previousDate = ConvertJalaliToGregorian(previousDateJalali);
            DateOnly currentDate = ConvertJalaliToGregorian(currentDateJalali);

            List<string> s = new List<string>();
            foreach (var item in allNerkh)
            {
                double partialConsumption = CalcPartial(item, previousDate, currentDate, dailyAverage);
            }
        }
       
    }
}