using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Implementations;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class CalculationConfirmationHandler : AbstractBaseConnection, ICalculationConfirmationHandler
    {
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly IOldTariffEngine _oldTariffEngine;
        private readonly IVariabService _variabService;
        const int _paymentDeadline = 7;

        public CalculationConfirmationHandler(
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
            IMeterReadingDetailQueryService meterReadingDetailService,
            IMeterFlowQueryService meterFlowQueryService,
            IOldTariffEngine oldTariffEngine,
            IVariabService variabService,
            IConfiguration configuration)
            : base(configuration)
        {
            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));

            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));

            _oldTariffEngine = oldTariffEngine;
            _oldTariffEngine.NotNull(nameof(oldTariffEngine));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));
        }

        public async Task<MeterReadingCheckedOutputDto> Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(latestFlowId, MeterFlowStepEnum.ConsumptionChecked, cancellationToken);

            int firstFlowId = await _meterFlowQueryService.GetFirstFlowId(latestFlowId);
            IEnumerable<MeterReadingDetailDataOutputDto> meterReadings = await _meterReadingDetailService.Get(firstFlowId);

            int zoneId = meterReadings.FirstOrDefault().ZoneId;
            var (bedBesBatch, kasrHaBatch) = await GetBedBesAndKasrHaDto(meterReadings, cancellationToken);
            int newMeterFlowId = await CommandTransactions(bedBesBatch, kasrHaBatch, zoneId, latestFlowId, appUser);

            return GetResult(newMeterFlowId);
        }
        private async Task<(ICollection<BedBesCreateDto>, ICollection<KasrHaDto>)> GetBedBesAndKasrHaDto(IEnumerable<MeterReadingDetailDataOutputDto> meterReadings, CancellationToken cancellationToken)
        {
            ICollection<BedBesCreateDto> BedBesBatch = new List<BedBesCreateDto>();
            ICollection<KasrHaDto> kasrHaBatch = new List<KasrHaDto>();
            foreach (var mr in meterReadings)
            {
                BedBesCreateDto bedBes = await GetBedBes(mr);
                BedBesBatch.Add(bedBes);

                if (mr.DiscountSum > 0)
                {
                    KasrHaDto kasrHa = GerKasrHa(mr);
                    kasrHaBatch.Add(kasrHa);
                }
            }
            return (BedBesBatch, kasrHaBatch);
        }
        private async Task<int> CommandTransactions(ICollection<BedBesCreateDto> BedBesBatch, ICollection<KasrHaDto> kasrHaBatch, int zoneId, int latestFlowId, IAppUser appUser)
        {
            //string dbName = GetDbName(zoneId);
            string dbName = "Atlas";
            MeterFlowGetDto meterFlow = await _meterFlowQueryService.Get(latestFlowId);
            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            MeterFlowCreateDto newMeterFlow = new()
            {
                MeterFlowStepId = MeterFlowStepEnum.CalculationConfirmed,
                ZoneId = meterFlow.ZoneId,
                FileName = meterFlow.FileName,
                InsertByUserId = appUser.UserId,
                InsertDateTime = DateTime.Now,
                Description = meterFlow.Description
            };

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    BedBesCommandService bedBesCreateService = new BedBesCommandService(connection, transaction);
                    KasrHaCommandService kasrHaCommandService = new KasrHaCommandService(connection, transaction);
                    MeterFlowCommandService meterFlowCommandService = new(connection, transaction);

                    await bedBesCreateService.InsertByBulk(BedBesBatch, dbName);
                    await kasrHaCommandService.InsertByBulk(kasrHaBatch, dbName);

                    await meterFlowCommandService.Update(meterFlowUpdate);
                    int newMeterFlowId = await meterFlowCommandService.Insert(newMeterFlow);

                    transaction.Commit();
                    return newMeterFlowId;
                }
            }
        }
        private async Task<BedBesCreateDto> GetBedBes(MeterReadingDetailDataOutputDto meterReading)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string mohlatDateJalali = DateTime.Now.AddDays(_paymentDeadline).ToShortPersianDateString();
            decimal barge = await _variabService.GetAndRenew(meterReading.ZoneId);
            string paymentId = TransactionIdGenerator.GeneratePaymentId((long)meterReading.Pard, meterReading.BillId);//todo


            return new BedBesCreateDto()
            {
                Town = meterReading.ZoneId,
                Radif = meterReading.CustomerNumber,
                Eshtrak = meterReading.ReadingNumber,
                Barge = barge,
                PriNo = meterReading.PreviousNumber,
                TodayNo = meterReading.CurrentNumber,
                PriDate = meterReading.PreviousDateJalali,
                TodayDate = meterReading.CurrentDateJalali,
                AbonFas = (decimal)meterReading.AbonFas,
                FasBaha = (decimal)meterReading.FasBaha,
                AbBaha = (decimal)meterReading.AbBaha,
                Ztadil = (decimal)meterReading.Ztadil,
                Masraf = (decimal)meterReading.Consumption,
                Shahrdari = (decimal)meterReading.Shahrdari,
                Modat = meterReading.Modat ?? 0,
                DateBed = currentDateJalali,
                JalaseNo = 0,//todo
                Mohlat = mohlatDateJalali,
                Baha = (decimal)meterReading.SumItems,
                AbonAb = (decimal)meterReading.AbonAb,
                Pard = (decimal)(Math.Round(meterReading.SumItems.Value, 3)),//bedehi gahbli+currentSumItems  
                Jam = (decimal)meterReading.SumItems,//bedehi gahbli+currentSumItems  
                CodVas = meterReading.CurrentCounterStateCode,
                Ghabs = "1",
                Del = false,
                Type = "1",
                CodEnshab = meterReading.UsageId,
                Enshab = meterReading.MeterDiameterId,
                Elat = 0,
                Serial = 0,
                Ser = 0,
                ZaribFasl = (decimal)meterReading.ZaribFasl,
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = meterReading.OtherUnit,
                TedKhane = meterReading.HouseholdNumber,
                TedadMas = meterReading.DomesticUnit,
                TedadTej = meterReading.CommercialUnit,
                NoeVa = meterReading.BranchTypeId,
                Jarime = 0,
                Masjar = 0,
                Sabt = 1,
                Rate = (decimal)meterReading.MonthlyConsumption,
                Operator = 0,
                Mamor = meterReading.AgentCode,
                TavizDate = meterReading.TavizDateJalali ?? string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = (decimal)meterReading.ZaribD,
                Tafavot = 0,
                KasrHa = (decimal)meterReading.DiscountSum,
                FixMas = meterReading.ContractualCapacity,
                ShGhabs1 = meterReading.BillId,
                ShPard1 = paymentId,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                NewAb = 0,
                NewFa = 0,
                Bodjeh = (decimal)meterReading.Bodjeh,
                Group1 = meterReading.ConsumptionUsageId,
                MasFas = (decimal)meterReading.Consumption,
                Faz = false,
                ChkKarbari = (decimal)meterReading.ChkKarbari,
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
                Avarez = (decimal)meterReading.Avarez,
                TrackNumber = long.Parse(paymentId)//Todo
            };
        }
        private KasrHaDto GerKasrHa(MeterReadingDetailDataOutputDto meterReading)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string paymentId = TransactionIdGenerator.GeneratePaymentId((long)meterReading.SumItems, meterReading.BillId);//todo

            return new KasrHaDto()
            {
                Town = meterReading.ZoneId,
                IdBedbes = 0,
                Radif = meterReading.CustomerNumber,
                CodEnshab = meterReading.UsageId,
                Barge = 0,
                PriDate = meterReading.PreviousDateJalali,
                TodayDate = meterReading.CurrentDateJalali,
                PriNo = meterReading.PreviousNumber,
                TodayNo = meterReading.CurrentNumber,
                Masraf = (decimal)meterReading.Consumption,
                AbBaha = (decimal)meterReading.AbBahaDiscount,
                FasBaha = (decimal)meterReading.FazelabDiscount,
                AbonAb = (decimal)meterReading.AbonmanAbDiscount,
                AbonFas = (decimal)meterReading.AbonmanFazelabDiscount,
                TabAbnA = 0,
                TabAbnF = 0,
                Ab10 = 0,
                Shahrdari = (decimal)meterReading.MaliatDiscount,
                Rate = (decimal)meterReading.MonthlyConsumption,
                Baha = (decimal)meterReading.DiscountSum,
                ShGhabs = meterReading.BillId,
                ShPard = paymentId,//todo
                DateBed = currentDateJalali,
                TmpDateBed = "",
                TmpTodayDate = "",
                TedVahd = meterReading.OtherUnit,
                TedKhane = meterReading.HouseholdNumber,
                TedadMas = meterReading.DomesticUnit,
                TedadTej = meterReading.CommercialUnit,
                ZaribFasl = 0,
                NoeVa = meterReading.BranchTypeId,
                Bodjeh = (decimal)meterReading.BoodjeDiscount,
            };
        }
        private async Task<int> CreateCalculationConfirmedFlow(IDbConnection connection, IDbTransaction transaction, int latestFlowId, IAppUser appUser)
        {
            MeterFlowCommandService meterFlowCommandService = new(connection, transaction);

            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            await meterFlowCommandService.Update(meterFlowUpdate);

            MeterFlowGetDto meterFlow = await _meterFlowQueryService.Get(latestFlowId);
            MeterFlowCreateDto newMeterFlow = new()
            {
                MeterFlowStepId = MeterFlowStepEnum.CalculationConfirmed,
                ZoneId = meterFlow.ZoneId,
                FileName = meterFlow.FileName,
                InsertByUserId = appUser.UserId,
                InsertDateTime = DateTime.Now,
                Description = meterFlow.Description
            };
            int newMeterFlowId = await meterFlowCommandService.Insert(newMeterFlow);
            return newMeterFlowId;
        }
        private MeterImaginaryInputDto GetMeterImaginary(MeterReadingDetailDataOutputDto readingDetail)
        {
            CustomerDetailInfoInputDto customerInfo = new()
            {
                ZoneId = readingDetail.ZoneId,
                Radif = readingDetail.CustomerNumber,
                BranchType = readingDetail.BranchTypeId,
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
        private MeterReadingCheckedOutputDto GetResult(int flowId)
        {
            return new MeterReadingCheckedOutputDto(flowId, MeterFlowStepEnum.ClientNotification, MessageLiterals.SuccessfullOperation);
        }
        private bool CounterStateValidation(int counterStateCode, int currentNumber, int previousNumber)
        {
            int[] invalidCounterStateCode = [4, /*6,*/ 7, 8, 9, 10];

            if (counterStateCode == 6 && previousNumber != currentNumber)
            {
                return false;
            }
            if (invalidCounterStateCode.Contains(counterStateCode))
            {
                return false;
            }
            else if ((counterStateCode == 3 || counterStateCode == 5) && currentNumber > previousNumber)
            {
                return false;
            }
            return true;
        }
    }
}
