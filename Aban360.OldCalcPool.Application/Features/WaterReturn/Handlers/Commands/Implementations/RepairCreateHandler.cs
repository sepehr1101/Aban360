using Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Contracts;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;
using Aban360.OldCalcPools.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;

namespace Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class RepairCreateHandler : IRepairCreateHandler
    {
        static int invoiceDeadline = 7;
        private readonly IRepairCommandService _commandService;
        private readonly IMembersQueryService _membersQueryService;
        private readonly IOldTariffEngine _tariffEngine;
        private readonly IBillQueryService _billQueryService;
        public RepairCreateHandler(
            IRepairCommandService commandService,
            IMembersQueryService membersQueryService,
            IOldTariffEngine tariffEngine,
            IBillQueryService billQueryService)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _membersQueryService = membersQueryService;
            _membersQueryService.NotNull(nameof(membersQueryService));

            _tariffEngine = tariffEngine;
            _tariffEngine.NotNull(nameof(tariffEngine));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));
        }
        public async Task Handle(OfferingToCreateRepairDto input, CancellationToken cancellationToken)
        {
            MemberGetDto memberInfo = await _membersQueryService.Get(input.BillId);
            RepairCreateDto repairDto = GetRepairCreateDto(input, memberInfo);
            await _commandService.Create(repairDto);
        }
        public async Task<AbBahaCalculationDetails> Handle(MeterInfoWithMonthlyConsumptionOutputDto input, CancellationToken cancellationToken)
        {
            BedBesDataInfoOutptuDto bedBesData = await _billQueryService.Get(input.InvoiceId);
            ValidationBedBes(bedBesData, input.CurrentDateJalali, input.PreviousDateJalali);

            MeterDateInfoWithMonthlyConsumptionOutputDto meterDateInfo = new MeterDateInfoWithMonthlyConsumptionOutputDto(input.BillId, input.PreviousDateJalali, input.CurrentDateJalali, input.MonthlyAverageConsumption);
            AbBahaCalculationDetails tariff = await _tariffEngine.Handle(meterDateInfo, cancellationToken);
            if (!input.ShouldSave)
            {
                return tariff;
            }
            RepairCreateDto repairDto = GetRepairCreateDto(tariff, bedBesData, input.MeetingNumber, input.Cause);
            await _commandService.Create(repairDto);
            return tariff;
        }
        public async Task<AbBahaCalculationDetails> Handle(MeterInfoByLastMonthlyConsumptionOutputDto input, CancellationToken cancellationToken)
        {
            BedBesDataInfoOutptuDto bedBesData = await _billQueryService.Get(input.InvoiceId);
            ValidationBedBes(bedBesData, input.CurrentDateJalali, input.PreviousDateJalali);

            MeterDateInfoByLastMonthlyConsumptionOutputDto meterDateInfo = new MeterDateInfoByLastMonthlyConsumptionOutputDto(input.BillId, input.PreviousDateJalali, input.CurrentDateJalali);
            AbBahaCalculationDetails tariff = await _tariffEngine.Handle(meterDateInfo, cancellationToken);
            if (!input.ShouldSave)
            {
                return tariff;
            }
            RepairCreateDto repairDto = GetRepairCreateDto(tariff, bedBesData, input.MeetingNumber, input.Cause);
            await _commandService.Create(repairDto);
            return tariff;
        }
        public async Task<AbBahaCalculationDetails> Handle(MeterInfoByPreviousDataWithInvoiceIdInputDto input, CancellationToken cancellationToken)
        {
            BedBesDataInfoOutptuDto bedBesData = await _billQueryService.Get(input.InvoiceId);
            ValidationBedBes(bedBesData, input.CurrentDateJalali, input.PreviousDateJalali);

            MeterInfoByPreviousDataInputDto meterInfoByPreviousData = GetMeterInfoByPreviousData(input);
            AbBahaCalculationDetails tariff = await _tariffEngine.Handle(meterInfoByPreviousData, cancellationToken);
            if (!input.ShouldSave)
            {
                return tariff;
            }
            RepairCreateDto repairDto = GetRepairCreateDto(tariff, bedBesData, input.MeetingNumber, input.Cause);
            await _commandService.Create(repairDto);
            return tariff;
        }
        private void ValidationBedBes(BedBesDataInfoOutptuDto bedBesData, string currentDateJalali, string previousDateJalali)
        {
            if (bedBesData == null ||
                bedBesData.PreviousDateJalali != previousDateJalali ||
                bedBesData.CurrentDateJalali != currentDateJalali)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidBillId);
            }

        }
        private RepairCreateDto GetRepairCreateDto(AbBahaCalculationDetails tariffInfo, BedBesDataInfoOutptuDto bedBesInfo, decimal meetingNumber, int cause)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string invoiceDeadlineDateJalali = DateTime.Now.AddDays(invoiceDeadline).ToShortPersianDateString();

            return new RepairCreateDto()
            {
                Town = tariffInfo.Customer.ZoneId,
                Radif = tariffInfo.Customer.Radif,
                Eshtrak = tariffInfo.Customer.ReadingNumber,
                Barge = 0,
                PriNo = tariffInfo.MeterInfo.PreviousNumber,
                TodayNo = tariffInfo.MeterInfo.CurrentNumber,
                PriDate = tariffInfo.MeterInfo.PreviousDateJalali,
                TodayDate = tariffInfo.MeterInfo.CurrentDateJalali,
                AbonFas = (decimal)tariffInfo.AbonmanFazelabAmount,
                FasBaha = (decimal)tariffInfo.FazelabAmount,
                AbBaha = (decimal)tariffInfo.AbBahaAmount,
                Ztadil = 0,
                Masraf = (decimal)tariffInfo.Consumption,
                Shahrdari = (decimal)tariffInfo.AvarezAmount,
                Modat = (decimal)tariffInfo.Duration,
                DateBed = bedBesInfo.DateBed,
                JalaseNo = meetingNumber,
                Mohlat = invoiceDeadlineDateJalali,
                Baha = (decimal)tariffInfo.SumItems,
                AbonAb = (decimal)tariffInfo.AbonmanAbAmount,
                Pard = (decimal)tariffInfo.SumItems,
                Jam = (decimal)tariffInfo.SumItems,
                CodVas = (decimal)bedBesInfo.CounterStateCode,
                Ghabs = string.Empty,
                Del = false,
                Type = "4",
                CodEnshab = tariffInfo.Customer.UsageId,
                Enshab = tariffInfo.Customer.MeterDiameterId,
                Elat = cause,
                Serial = bedBesInfo.BodySerial,
                Ser = 0,
                ZaribFasl = 0,//
                Ab10 = 0,
                Ab20 = 0,
                TedadMas = (decimal)tariffInfo.Customer.DomesticUnit,
                TedadTej = (decimal)tariffInfo.Customer.CommertialUnit,
                TedadVahd = (decimal)tariffInfo.Customer.OtherUnit,
                NoeVa = (decimal)tariffInfo.Customer.BranchType,
                Jarime = 0,//
                Masjar = 0,
                Sabt = 0,
                Rate = (decimal)tariffInfo.MonthlyConsumption,
                Operator = 0,
                Mamor = 0,
                TavizDate = string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = 0,//
                Tafavot = 0,//
                MasHadar = 0,
                AbHadar = 0,
                RangeMas = 0,//
                TafBack = 0,
                TedGhabs = 0,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                Bodjeh = (decimal)tariffInfo.SumBoodje,
                Group1 = (decimal)tariffInfo.Customer.UsageId,
                Faz = tariffInfo.Customer.SewageCalcState > 0 ? true : false,
                ChkKarbari = 0,//
                C200 = 0,//
                TmpPriDate = string.Empty,
                TmpTodayDate = string.Empty,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                TmpDateBed = string.Empty,
                EdarehK = false,//
                Lavazem = 0,//
                DateSbt = string.Empty,
                Avarez = (decimal)tariffInfo.AvarezAmount
            };
        }
        private RepairCreateDto GetRepairCreateDto(OfferingToCreateRepairDto offeringInfo, MemberGetDto memberInfo)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string invoiceDeadlineDateJalali = DateTime.Now.AddDays(invoiceDeadline).ToShortPersianDateString();
            string currentDateJalali10Char = currentDateJalali.Substring(2);
            decimal sumOfferings = offeringInfo.AbonFas + offeringInfo.FasBaha + offeringInfo.AbBaha + offeringInfo.Ztadil + offeringInfo.Shahrdari + offeringInfo.AbonAb + offeringInfo.ZaribFasl + offeringInfo.Ab10 + offeringInfo.Ab20 + offeringInfo.Zabresani + offeringInfo.TabAbnA + offeringInfo.TabAbnF + offeringInfo.TabsFa + offeringInfo.Bodjeh + offeringInfo.Group1 + offeringInfo.Avarez;

            return new RepairCreateDto()
            {
                Town = memberInfo.ZoneId,
                Radif = memberInfo.CustomerNumber,
                Eshtrak = memberInfo.ReadingNumber,
                Barge = 0,
                PriNo = 0,
                TodayNo = 0,
                PriDate = string.Empty,
                TodayDate = string.Empty,
                AbonFas = offeringInfo.AbonFas,
                FasBaha = offeringInfo.FasBaha,
                AbBaha = offeringInfo.AbBaha,
                Ztadil = offeringInfo.Ztadil,
                Masraf = 0,
                Shahrdari = offeringInfo.Shahrdari,
                Modat = 0,
                DateBed = currentDateJalali,
                JalaseNo = offeringInfo.JalaseNo,
                Mohlat = invoiceDeadlineDateJalali,
                Baha = sumOfferings,
                AbonAb = offeringInfo.AbonAb,
                Pard = sumOfferings,
                Jam = sumOfferings,
                CodVas = 0,
                Ghabs = string.Empty,
                Del = false,
                Type = "4",
                CodEnshab = memberInfo.UsageId,
                Enshab = memberInfo.MeterDiamterId,
                Elat = offeringInfo.Elat,
                Serial = 0,//memberInfo.BodySerial,
                Ser = 0,
                ZaribFasl = offeringInfo.ZaribFasl,
                Ab10 = offeringInfo.Ab10,
                Ab20 = offeringInfo.Ab20,
                TedadVahd = memberInfo.OtherUnit,
                TedadMas = memberInfo.DomesticUnit,
                TedadTej = memberInfo.CommertialUnit,
                TedKhane = memberInfo.HouseholdNumber,
                NoeVa = memberInfo.BranchTypeId,
                Jarime = 0,
                Masjar = 0,
                Sabt = 0,
                Rate = 0,
                Operator = 0,
                Mamor = 0,
                TavizDate = string.Empty,
                ZaribCntr = 0,
                Zabresani = offeringInfo.Zabresani,
                ZaribD = offeringInfo.ZaribD,
                Tafavot = 0,
                MasHadar = 0,
                AbHadar = 0,
                RangeMas = 0,
                TafBack = 0,
                TedGhabs = 0,
                TabAbnA = offeringInfo.TabAbnA,
                TabAbnF = offeringInfo.TabAbnF,
                TabsFa = offeringInfo.TabsFa,
                Bodjeh = offeringInfo.Bodjeh,
                Group1 = offeringInfo.Group1,
                Faz = offeringInfo.Faz,
                ChkKarbari = 0,
                C200 = 0,
                TmpPriDate = string.Empty,
                TmpTodayDate = currentDateJalali10Char,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                TmpDateBed = currentDateJalali10Char,
                EdarehK = false,
                Lavazem = 0,
                DateSbt = string.Empty,
                Avarez = offeringInfo.Avarez,
            };
        }
        private MeterInfoByPreviousDataInputDto GetMeterInfoByPreviousData(MeterInfoByPreviousDataWithInvoiceIdInputDto input)
        {
            return new MeterInfoByPreviousDataInputDto()
            {
                BillId = input.BillId,
                CurrentDateJalali = input.CurrentDateJalali,
                PreviousDateJalali = input.PreviousDateJalali,
                CurrentMeterNumber = input.CurrentMeterNumber,
                PreviousNumber = input.PreviousNumber
            };
        }
    }
}
