using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class CalculationConfirmedHandler : ICalculationConfirmedHandler
    {
        private readonly IMeterReadingDetailService _meterReadingDetailService;
        private readonly IMeterFlowService _meterFlowService;
        private readonly IBedBesCreateService _bedBesCreateService;
        public CalculationConfirmedHandler(
            IMeterReadingDetailService meterReadingDetailService,
            IMeterFlowService meterFlowService,
            IBedBesCreateService bedBesCreateService)
        {
            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));

            _bedBesCreateService= bedBesCreateService;
            _bedBesCreateService.NotNull(nameof(bedBesCreateService));
        }

        public async Task Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            int firstFlowId = await _meterFlowService.GetFirstFlowId(latestFlowId);
            IEnumerable<MeterReadingDetailGetDto> meterReadings = await _meterReadingDetailService.Get(firstFlowId);
            await CreateCalculationConfirmedFlow(latestFlowId, appUser);
            await CreateBedBesBatch(meterReadings);//Insert in Atlas.dbo.BedBes
        }
        private async Task CreateBedBesBatch(IEnumerable<MeterReadingDetailGetDto> meterReadings)
        {
            ICollection<BedBesCreateDto> BedBesBatch=new List<BedBesCreateDto>();
            meterReadings.ForEach(mr =>
            {
                BedBesCreateDto bedBes=GetBedBes(mr);
                BedBesBatch.Add(bedBes);
            });

            await _bedBesCreateService.Create(BedBesBatch, meterReadings.FirstOrDefault().ZoneId);
        }
        private BedBesCreateDto GetBedBes(MeterReadingDetailGetDto s)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string mohlatDateJalali = DateTime.Now.AddDays(10).ToShortPersianDateString();

            return new BedBesCreateDto()
            {
                Town = s.ZoneId,
                Radif = s.CustomerNumber,
                Eshtrak = s.ReadingNumber,
                Barge = 0,//
                PriNo = s.PreviousNumber,
                TodayNo = s.CurrentNumber,
                PriDate = s.PreviousDateJalali,
                TodayDate = s.CurrentDateJalali,
                AbonFas = 0,//
                FasBaha = 0,//
                AbBaha = 0,//
                Ztadil = 0,//
                Masraf = (decimal)s.Consumption,
                Shahrdari = 0,//
                Modat = 0,//todoooo
                DateBed = currentDateJalali,
                JalaseNo = 0,//
                Mohlat = mohlatDateJalali,
                Baha = (decimal)s.SumItems,
                AbonAb = 0,//
                Pard = (decimal)(Math.Round(s.SumItems.Value, 3)),
                Jam = (decimal)s.SumItems,
                CodVas = s.CurrentCounterStateCode,
                Ghabs = "0",//
                Del = false,
                Type = "1",
                CodEnshab = s.UsageId,
                Enshab = s.MeterDiameterId,
                Elat = 0,
                Serial = 0,
                Ser = 0,
                ZaribFasl = 0,
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = s.OtherUnit,
                TedKhane = s.HouseholdNumber,
                TedadMas = s.DomesticUnit,
                TedadTej = s.CommercialUnit,
                NoeVa = 0,//todooo s.branchType
                Jarime = 0,
                Masjar = 0,
                Sabt = 1,
                Rate = (decimal)s.MonthlyConsumption,
                Operator = 0,
                Mamor = s.AgentCode,
                TavizDate = s.TavizDateJalali??string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = 0,
                Tafavot = 0,
                KasrHa = 0,//
                FixMas = 0,//
                ShGhabs1 = s.BillId,
                ShPard1 = "0",//
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                NewAb = 0,
                NewFa = 0,
                Bodjeh = 0,
                Group1 = 0,
                MasFas = 0,
                Faz = false,
                ChkKarbari = 0,
                C200 = 0,
                DateIns = "",
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
                KhaliS = s.EmptyUnit,
                EdarehK = s.IsSpecial,
                Tafa402 = 0,
                Avarez = 0,
                TrackNumber = 0
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
    }
}
