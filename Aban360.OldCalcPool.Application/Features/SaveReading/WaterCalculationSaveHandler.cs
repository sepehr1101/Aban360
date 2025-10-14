using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Application.Features.SaveReading
{
    public interface IWaterCalculationSaveHandler
    {
        Task Handle(IEnumerable<ReadingBillInputDto> inputDto, int mamorCode, CancellationToken cancellationToken);
    }
    internal sealed class WaterCalculationSaveHandler : IWaterCalculationSaveHandler
    {
        private readonly IProcessing _processing;
        private readonly IBedBesCreateService _bedBesCreateService;
        public WaterCalculationSaveHandler(
            IProcessing processing,
            IBedBesCreateService bedBesCreateService)
        {
            _processing = processing;
            _processing.NotNull(nameof(processing));

            _bedBesCreateService = bedBesCreateService;
            _bedBesCreateService.NotNull(nameof(bedBesCreateService));
        }

        public async Task Handle(IEnumerable<ReadingBillInputDto> inputDto, int mamorCode, CancellationToken cancellationToken)
        {
            ICollection<AbBahaCalculationDetails> calculationDetails = new List<AbBahaCalculationDetails>();
            ICollection<BedBesCreateDto> bedBes = new List<BedBesCreateDto>();
            foreach (var customer in inputDto)
            {
                MeterInfoInputDto meterInfo = new MeterInfoInputDto
                {
                    BillId = customer.BillId,
                    CurrentDateJalali = customer.ReadingDateJalali,
                    CurrentMeterNumber = customer.MeterNumber,
                };
                AbBahaCalculationDetails result = await _processing.HandleWithAggregatedNerkh(meterInfo, cancellationToken);
                calculationDetails.Add(result);
                bedBes.Add(await CreateBedBes(result, customer, mamorCode));
            }
            await _bedBesCreateService.Create(bedBes);
        }

        private async Task<BedBesCreateDto> CreateBedBes(AbBahaCalculationDetails s, ReadingBillInputDto readingBillInfo, int mamorCode)
        {
            string dateNowJalali = DateTime.Now.ToShortPersianDateString();
            return new BedBesCreateDto()
            {
                Town = s.Customer.ZoneId,
                Radif = s.Customer.Radif,
                Eshtrak = s.Customer.ReadingNumber,
                Barge = 1,
                PriNo = s.MeterInfo.PreviousNumber,
                TodayNo = readingBillInfo.MeterNumber,
                PriDate = s.MeterInfo.PreviousDateJalali,
                TodayDate = readingBillInfo.ReadingDateJalali,
                AbonFas = (decimal)s.AbonmanFazelabAmount,
                FasBaha = (decimal)s.FazelabAmount,
                AbBaha = (decimal)s.AbBahaAmount,
                //Ztadil = (decimal)s.Zarib,
                Masraf = (decimal)s.Consumption,
                Shahrdari = 0,
                Modat = s.Duration,
                DateBed = DateTime.Now.ToShortPersianDateString(),//
                JalaseNo = 0,//
                Mohlat = GetForwardDateJalali(dateNowJalali, 10, s.Customer.BillId),
                Baha = (decimal)s.SumItems,
                AbonAb = (decimal)s.AbonmanAbAmount,
                Pard = Math.Round((decimal)s.SumItems, 3),
                Jam = (decimal)s.SumItems,//
                CodVas = (decimal)readingBillInfo.CounterStateId,
                Ghabs = "",
                Del = false,
                Type = "1",
                CodEnshab = s.Customer.UsageId,
                Enshab = s.Customer.MeterDiameterId,
                Elat = 0,
                Serial = 0,
                Ser = 0,
                ZaribFasl = (decimal)s.HotSeasonAbBahaAmount,//
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = s.Customer.OtherUnit,
                TedKhane = s.Customer.HouseholdNumber,
                TedadMas = s.Customer.DomesticUnit,
                TedadTej = s.Customer.CommertialUnit,
                NoeVa = s.Customer.BranchType,
                Jarime = 0,
                Masjar = 0,
                Sabt = 1,
                Rate = (decimal)s.MonthlyConsumption,
                Operator = 0,
                Mamor = mamorCode,
                TavizDate = "",//
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = 0,
                Tafavot = 0,
                KasrHa = (decimal)s.DiscountSum,
                FixMas = (decimal)s.Consumption,//
                ShGhabs1 = s.Customer.BillId,
                ShPard1 = "must generate",
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                NewAb = 0,
                NewFa = 0,
                Bodjeh = (decimal)s.SumBoodje,
                Group1 = 0,
                MasFas = 0,
                Faz = s.FazelabAmount > 0 ? true : false,
                ChkKarbari = 0,
                C200 = 0,
                DateIns = dateNowJalali.Substring(dateNowJalali.Length - 8),
                AbSevom = 0,
                AbSevom1 = 0,
                C70 = 0,
                C80 = 0,
                TmpDateBed = "",
                TmpPriDate = "",
                TmpTodayDate = "",
                TmpMohlat = "",
                TmpTavizDate = "",
                C90 = 0,
                C101 = 0,
                KhaliS = s.Customer.EmptyUnit,
                EdarehK = s.Customer.IsSpecial,
                Avarez = (decimal)s.AvarezAmount,
                TrackNumber = 0
            };
        }

        public string GetForwardDateJalali(string date, int day, string billId)
        {
            DateOnly? gregorianDate = date.ToGregorianDateOnly();
            if (!gregorianDate.HasValue)
                throw new TariffDateException(ExceptionLiterals.Incalculable + "-" + billId);

            DateOnly forwardDate = gregorianDate.Value.AddDays(day);
            return forwardDate.ToShortPersianDateString();
        }
    }
    public record ReadingBillInputDto
    {
        public string BillId { get; set; }
        public int ZoneId { get; set; }
        public string ReadingDateJalali { get; set; }
        public int MeterNumber { get; set; }
        public int CounterStateId { get; set; }

    }
}
