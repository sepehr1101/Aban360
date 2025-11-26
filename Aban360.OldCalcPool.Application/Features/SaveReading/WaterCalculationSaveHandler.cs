using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using DNTPersianUtils.Core;
using Microsoft.Data.SqlClient;

namespace Aban360.OldCalcPool.Application.Features.SaveReading
{
    public interface IWaterCalculationSaveHandler
    {
        Task Handle(IEnumerable<ReadingBillInputDto> inputDto, int mamorCode, CancellationToken cancellationToken);
    }
    internal sealed class WaterCalculationSaveHandler : IWaterCalculationSaveHandler
    {
        private readonly IOldTariffEngine _processing;
        private readonly IBedBesCreateService _bedBesCreateService;
        private readonly IKasrHaService _kasrHaService;
        public WaterCalculationSaveHandler(
            IOldTariffEngine processing,
            IBedBesCreateService bedBesCreateService,
            IKasrHaService kasrHaService)
        {
            _processing = processing;
            _processing.NotNull(nameof(processing));

            _bedBesCreateService = bedBesCreateService;
            _bedBesCreateService.NotNull(nameof(bedBesCreateService));

            _kasrHaService = kasrHaService;
            _kasrHaService.NotNull(nameof(kasrHaService));
        }

        public async Task Handle(IEnumerable<ReadingBillInputDto> inputDto, int mamorCode, CancellationToken cancellationToken)
        {
            ICollection<AbBahaCalculationDetails> calculationDetails = new List<AbBahaCalculationDetails>();
            ICollection<BedBesCreateDto> bedBes = new List<BedBesCreateDto>();
            ICollection<KasrHaDto> kasrHa = new List<KasrHaDto>();
            foreach (var customer in inputDto)
            {
                MeterInfoInputDto meterInfo = new MeterInfoInputDto
                {
                    BillId = customer.BillId,
                    CurrentDateJalali = customer.ReadingDateJalali,
                    CurrentMeterNumber = customer.MeterNumber,
                };
                AbBahaCalculationDetails result = await _processing.Handle(meterInfo, cancellationToken);
                calculationDetails.Add(result);
                bedBes.Add(CreateBedBes(result, customer, mamorCode));
                kasrHa.Add(CreateKasrha(result, customer));
            }
            await _bedBesCreateService.Create(bedBes);
            await _kasrHaService.Create(kasrHa);
        }

        private BedBesCreateDto CreateBedBes(AbBahaCalculationDetails abBahaCalculation, ReadingBillInputDto readingBillInfo, int mamorCode)
        {
            string dateNowJalali = DateTime.Now.ToShortPersianDateString();
            return new BedBesCreateDto()
            {
                Town = abBahaCalculation.Customer.ZoneId,
                Radif = abBahaCalculation.Customer.Radif,
                Eshtrak = abBahaCalculation.Customer.ReadingNumber,
                Barge = 1,
                PriNo = abBahaCalculation.MeterInfo.PreviousNumber,
                TodayNo = readingBillInfo.MeterNumber,
                PriDate = abBahaCalculation.MeterInfo.PreviousDateJalali,
                TodayDate = readingBillInfo.ReadingDateJalali,
                AbonFas = (decimal)abBahaCalculation.AbonmanFazelabAmount,
                FasBaha = (decimal)abBahaCalculation.FazelabAmount,
                AbBaha = (decimal)abBahaCalculation.AbBahaAmount,
                //Ztadil = (decimal)s.Zarib,
                Masraf = (decimal)abBahaCalculation.Consumption,
                Shahrdari = 0,
                Modat = abBahaCalculation.Duration,
                DateBed = DateTime.Now.ToShortPersianDateString(),//
                JalaseNo = 0,//
                Mohlat = GetForwardDateJalali(dateNowJalali, 10, abBahaCalculation.Customer.BillId),
                Baha = (decimal)abBahaCalculation.SumItems,
                AbonAb = (decimal)abBahaCalculation.AbonmanAbAmount,
                Pard = Math.Round((decimal)abBahaCalculation.SumItems, 3),
                Jam = (decimal)abBahaCalculation.SumItems,//
                CodVas = (decimal)readingBillInfo.CounterStateId,
                Ghabs = "",
                Del = false,
                Type = "1",
                CodEnshab = abBahaCalculation.Customer.UsageId,
                Enshab = abBahaCalculation.Customer.MeterDiameterId,
                Elat = 0,
                Serial = 0,
                Ser = 0,
                ZaribFasl = (decimal)abBahaCalculation.HotSeasonAbBahaAmount,//
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = abBahaCalculation.Customer.OtherUnit,
                TedKhane = abBahaCalculation.Customer.HouseholdNumber,
                TedadMas = abBahaCalculation.Customer.DomesticUnit,
                TedadTej = abBahaCalculation.Customer.CommertialUnit,
                NoeVa = abBahaCalculation.Customer.BranchType,
                Jarime = 0,
                Masjar = 0,
                Sabt = 1,
                Rate = Math.Round((decimal)abBahaCalculation.MonthlyConsumption, 3),
                Operator = 0,
                Mamor = mamorCode,
                TavizDate = "",//
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = 0,
                Tafavot = 0,
                KasrHa = (decimal)abBahaCalculation.DiscountSum,
                FixMas = (decimal)abBahaCalculation.Consumption,//
                ShGhabs1 = abBahaCalculation.Customer.BillId,
                ShPard1 = "must generate",
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                NewAb = 0,
                NewFa = 0,
                Bodjeh = (decimal)abBahaCalculation.SumBoodje,
                Group1 = 0,
                MasFas = 0,
                Faz = abBahaCalculation.FazelabAmount > 0 ? true : false,
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
                KhaliS = abBahaCalculation.Customer.EmptyUnit,
                EdarehK = abBahaCalculation.Customer.IsSpecial,
                Avarez = (decimal)abBahaCalculation.AvarezAmount,
                TrackNumber = 0
            };
        }

        private KasrHaDto CreateKasrha(AbBahaCalculationDetails s, ReadingBillInputDto readingBillInfo)
        {
            string dateNowJalali = DateTime.Now.ToShortPersianDateString();
            return new KasrHaDto()
            {
                Town = s.Customer.ZoneId,
                IdBedbes = 1,
                Radif = s.Customer.Radif,
                CodEnshab = 0,//
                Barge = 0,
                PriDate = s.MeterInfo.PreviousDateJalali,
                TodayDate = readingBillInfo.ReadingDateJalali,
                PriNo = s.MeterInfo.PreviousNumber,
                TodayNo = readingBillInfo.MeterNumber,
                Masraf = (decimal)s.Consumption,
                AbBaha = (decimal)s.AbBahaAmount,
                FasBaha = (decimal)s.FazelabAmount,
                AbonAb = (decimal)s.AbonmanAbAmount,
                AbonFas = (decimal)s.AbonmanFazelabAmount,
                TabAbnA = 0,
                TabAbnF = 0,
                Ab10 = 0,
                Shahrdari = 0,
                Rate = 0,
                Baha = (decimal)s.SumItems,
                ShGhabs = s.Customer.BillId,
                ShPard = "",
                DateBed = "",
                TmpDateBed = "",
                TmpTodayDate = dateNowJalali.Substring(dateNowJalali.Length - 8),
                TedVahd = s.Customer.OtherUnit,//
                TedadTej = s.Customer.CommertialUnit,
                TedadMas = s.Customer.DomesticUnit,
                TedKhane = s.Customer.HouseholdNumber,
                ZaribFasl = 0,//
                NoeVa = s.Customer.BranchType,
                Bodjeh = (decimal)s.SumBoodje,
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
