using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using DNTPersianUtils.Core;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class CalculationConfirmedHandler : ICalculationConfirmedHandler
    {
        private readonly IMeterReadingDetailService _meterReadingDetailService;
        private readonly IMeterFlowService _meterFlowService;
        private readonly IOldTariffEngine _oldTariffEngine;
        private readonly IBedBesCreateService _bedBesCreateService;
        private readonly IKasrHaService _kasrHaService;
        public CalculationConfirmedHandler(
            IMeterReadingDetailService meterReadingDetailService,
            IMeterFlowService meterFlowService,
            IOldTariffEngine oldTariffEngine,
            IBedBesCreateService bedBesCreateService,
            IKasrHaService kasrHaService)
        {
            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));

            _oldTariffEngine = oldTariffEngine;
            _oldTariffEngine.NotNull(nameof(oldTariffEngine));

            _bedBesCreateService = bedBesCreateService;
            _bedBesCreateService.NotNull(nameof(bedBesCreateService));

            _kasrHaService = kasrHaService;
            _kasrHaService.NotNull(nameof(kasrHaService));
        }

        public async Task Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            int firstFlowId = await _meterFlowService.GetFirstFlowId(latestFlowId);
            IEnumerable<MeterReadingDetailGetDto> meterReadings = await _meterReadingDetailService.Get(firstFlowId);
            await CreateBedBesAndKasrHaBatch(meterReadings, cancellationToken);//Insert in Atlas.dbo.BedBes
            await CreateCalculationConfirmedFlow(latestFlowId, appUser);
        }
        private async Task CreateBedBesAndKasrHaBatch(IEnumerable<MeterReadingDetailGetDto> meterReadings, CancellationToken cancellationToken)
        {
            int zoneId = meterReadings.FirstOrDefault().ZoneId;
            ICollection<BedBesCreateDto> BedBesBatch = new List<BedBesCreateDto>();
            ICollection<KasrHaDto> kasrHaBatch = new List<KasrHaDto>();
            foreach (var mr in meterReadings)
            {
                int[] invalidCounterStateCode = [4, 6, 7, 8, 9, 10];
                if (invalidCounterStateCode.Contains(mr.CurrentCounterStateCode))
                {
                }
                else
                {
                    MeterImaginaryInputDto meterImaginary = GetMeterImaginary(mr);
                    AbBahaCalculationDetails abBahaCalc = await _oldTariffEngine.Handle(meterImaginary, cancellationToken);
                    BedBesCreateDto bedBes = GetBedBes(mr, abBahaCalc);
                    KasrHaDto kasrHa = GerKasrHa(mr, abBahaCalc);

                    BedBesBatch.Add(bedBes);
                    kasrHaBatch.Add(kasrHa);
                }
            }

            await _bedBesCreateService.Create(BedBesBatch,zoneId);
            await _kasrHaService.Create(kasrHaBatch, zoneId);
        }
        private BedBesCreateDto GetBedBes(MeterReadingDetailGetDto meterReading, AbBahaCalculationDetails abBahaCalc)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string mohlatDateJalali = DateTime.Now.AddDays(10).ToShortPersianDateString();

            return new BedBesCreateDto()
            {
                Town = meterReading.ZoneId,
                Radif = meterReading.CustomerNumber,
                Eshtrak = meterReading.ReadingNumber,
                Barge = 0,//
                PriNo = meterReading.PreviousNumber,
                TodayNo = meterReading.CurrentNumber,
                PriDate = meterReading.PreviousDateJalali,
                TodayDate = meterReading.CurrentDateJalali,
                AbonFas = (decimal)abBahaCalc.AbonmanFazelabAmount,
                FasBaha = (decimal)abBahaCalc.FazelabAmount,//
                AbBaha = (decimal)abBahaCalc.AbBahaAmount,//
                Ztadil = 0,//
                Masraf = (decimal)meterReading.Consumption,
                Shahrdari = (decimal)abBahaCalc.AvarezAmount,//
                Modat = abBahaCalc.Duration,//todoooo
                DateBed = currentDateJalali,
                JalaseNo = 0,//
                Mohlat = mohlatDateJalali,
                Baha = (decimal)meterReading.SumItems,
                AbonAb = (decimal)abBahaCalc.AbonmanAbAmount,//
                Pard = (decimal)(Math.Round(meterReading.SumItems.Value, 3)),
                Jam = (decimal)meterReading.SumItems,
                CodVas = meterReading.CurrentCounterStateCode,
                Ghabs = "0",//
                Del = false,
                Type = "1",
                CodEnshab = meterReading.UsageId,
                Enshab = meterReading.MeterDiameterId,
                Elat = 0,
                Serial = 0,
                Ser = 0,
                ZaribFasl = 0,
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = meterReading.OtherUnit,
                TedKhane = meterReading.HouseholdNumber,
                TedadMas = meterReading.DomesticUnit,
                TedadTej = meterReading.CommercialUnit,
                NoeVa = abBahaCalc.Customer.BranchType,//todooo s.branchType
                Jarime = 0,
                Masjar = 0,
                Sabt = 1,
                Rate = (decimal)meterReading.MonthlyConsumption,
                Operator = 0,
                Mamor = meterReading.AgentCode,
                TavizDate = meterReading.TavizDateJalali ?? string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = 0,
                Tafavot = 0,
                KasrHa = (decimal)abBahaCalc.DiscountSum,//
                FixMas = meterReading.ContractualCapacity,//
                ShGhabs1 = meterReading.BillId,
                ShPard1 = "0",//
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                NewAb = 0,
                NewFa = 0,
                Bodjeh = (decimal)abBahaCalc.SumBoodje,
                Group1 = 0,
                MasFas = 0,
                Faz = false,
                ChkKarbari = 0,//todo
                C200 = 0,
                DateIns = currentDateJalali,
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
                KhaliS = meterReading.EmptyUnit,
                EdarehK = meterReading.IsSpecial,
                Tafa402 = 0,
                Avarez = (decimal)abBahaCalc.AvarezAmount,
                TrackNumber = 0
            };
        }
        private KasrHaDto GerKasrHa(MeterReadingDetailGetDto meterReading, AbBahaCalculationDetails abBahaCalc)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();

            return new KasrHaDto()
            {
                Town = meterReading.ZoneId,
                IdBedbes = 0,
                Radif = meterReading.CustomerNumber,
                CodEnshab = meterReading.UsageId,
                Barge = 0,//
                PriDate = meterReading.PreviousDateJalali,
                TodayDate = meterReading.CurrentDateJalali,
                PriNo = meterReading.PreviousNumber,
                TodayNo = meterReading.CurrentNumber,
                Masraf = (decimal)meterReading.Consumption,
                AbBaha = (decimal)abBahaCalc.AbBahaDiscount,//
                FasBaha = (decimal)abBahaCalc.FazelabDiscount,//
                AbonAb = (decimal)abBahaCalc.AbonmanAbDiscount,//
                AbonFas = (decimal)abBahaCalc.AbonmanFazelabDiscount,
                TabAbnA = 0,
                TabAbnF = 0,
                Ab10 = 0,
                Shahrdari = (decimal)abBahaCalc.AvarezDiscount,//
                Rate = (decimal)meterReading.MonthlyConsumption,
                Baha = (decimal)meterReading.SumItems,
                ShGhabs = meterReading.BillId,
                ShPard = "",//todo
                DateBed = currentDateJalali,
                TmpDateBed = "",
                TmpTodayDate = "",
                TedVahd = meterReading.OtherUnit,
                TedKhane = meterReading.HouseholdNumber,
                TedadMas = meterReading.DomesticUnit,
                TedadTej = meterReading.CommercialUnit,
                ZaribFasl = 0,
                NoeVa = abBahaCalc.Customer.BranchType,
                Bodjeh = (decimal)abBahaCalc.BoodjeDiscount,
            };
        }
        private async Task CreateCalculationConfirmedFlow(int latestFlowId, IAppUser appUser)
        {
            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            _meterFlowService.Update(meterFlowUpdate);

            MeterFlowGetDto meterFlow = await _meterFlowService.Get(latestFlowId);
            MeterFlowCreateDto newMeterFlow = new()
            {
                MeterFlowStepId = MeterFlowStepEnum.CalculationConfirmed,
                ZoneId = meterFlow.ZoneId,
                FileName = meterFlow.FileName,
                InsertByUserId = appUser.UserId,
                InsertDateTime = DateTime.Now,
            };
            await _meterFlowService.Create(newMeterFlow);
        }
        private MeterImaginaryInputDto GetMeterImaginary(MeterReadingDetailGetDto readingDetail)
        {
            CustomerDetailInfoInputDto customerInfo = new()
            {
                ZoneId = readingDetail.ZoneId,
                Radif = readingDetail.CustomerNumber,
                BranchType = 0,//todo
                UsageId = readingDetail.UsageId,
                DomesticUnit = readingDetail.DomesticUnit,
                CommertialUnit = readingDetail.CommercialUnit,
                OtherUnit = readingDetail.OtherUnit,
                EmptyUnit = readingDetail.EmptyUnit,
                WaterInstallationDateJalali = readingDetail.WaterInstallationDateJalali,
                SewageInstallationDateJalali = readingDetail.SewageInstallationDateJalali,
                WaterRegisterDate = readingDetail.WaterRegisterDate,
                SewageRegisterDate = readingDetail.SewageRegisterDate,
                SewageCalcState = readingDetail.SewageCalcState,
                ContractualCapacity = readingDetail.ContractualCapacity,
                HouseholdDate = readingDetail.HouseholdDate,
                HouseholdNumber = readingDetail.HouseholdNumber,
                ReadingNumber = readingDetail.ReadingNumber,
                VillageId = readingDetail.VillageId,
                IsSpecial = readingDetail.IsSpecial,
                VirtualCategoryId = readingDetail.VirtualCategoryId,
                CounterStateCode = readingDetail.CurrentCounterStateCode,
            };
            MeterInfoByPreviousDataInputDto meterInfo = new()
            {
                BillId = readingDetail.BillId,
                PreviousDateJalali = readingDetail.PreviousDateJalali,
                PreviousNumber = readingDetail.PreviousNumber,
                CurrentDateJalali = readingDetail.CurrentDateJalali,
                CurrentMeterNumber = readingDetail.CurrentNumber,
                CounterStateCode = readingDetail.CurrentCounterStateCode
            };
            return new MeterImaginaryInputDto()
            {
                CustomerInfo = customerInfo,
                MeterPreviousData = meterInfo,
            };
        }
    }
}
