using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class ReturnBillFullHandler : IReturnBillFullHandler
    {
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IBedBesCommandService _bedBesCommandService;
        private readonly IRepairCommandService _repairCommandService;
        private readonly IAutoBackCommandService _autoBackCommandService;
        private readonly ICustomerInfoDetailQueryService _customerInfoDetailQueryService;
        private readonly IValidator<ReturnBillFullInputDto> _validator;
        public ReturnBillFullHandler(
            IBedBesQueryService bedBesQueryService,
            IBedBesCommandService bedBesCommandService,
            IRepairCommandService repairCommandService,
            IAutoBackCommandService autoBackCommandService,
            ICustomerInfoDetailQueryService customerInfoDetailQueryService,
            IValidator<ReturnBillFullInputDto> validator)
        {
            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _bedBesCommandService = bedBesCommandService;
            _bedBesCommandService.NotNull(nameof(bedBesCommandService));

            _repairCommandService = repairCommandService;
            _repairCommandService.NotNull(nameof(repairCommandService));

            _autoBackCommandService = autoBackCommandService;
            _autoBackCommandService.NotNull(nameof(autoBackCommandService));

            _customerInfoDetailQueryService = customerInfoDetailQueryService;
            _customerInfoDetailQueryService.NotNull(nameof(customerInfoDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReturnBillOutputDto> Handle(ReturnBillFullInputDto input, CancellationToken cancellationToken)
        {
            await ValidationFullInputDto(input, cancellationToken);
            CustomerInfoOutputDto customerInfo = await _customerInfoDetailQueryService.GetInfo(input.BillId);
            await FromToDateValidation(input, customerInfo);

            float previousConsumptionAverage = await _bedBesQueryService.GetPreviousBill(customerInfo.ZoneId, customerInfo.Radif, input.FromDateJalali);
            double consumptionAverage = input.UserInput is null ? previousConsumptionAverage : input.UserInput.Value;

            IEnumerable<BedBesCreateDto> bedBesInfo = await GetBedBesList(customerInfo, input);
            await UpdateBedBesDel(bedBesInfo);
            RepairCreateDto repairCreate = GetRepairCreateDto(bedBesInfo, customerInfo, input);
            AutoBackCreateDto autoBackCreate = GetAutoBackCreateDto(bedBesInfo, repairCreate);

            return new ReturnBillOutputDto(bedBesInfo, repairCreate, autoBackCreate);
        }
        private async Task<IEnumerable<BedBesCreateDto>> GetBedBesList(CustomerInfoOutputDto customerInfo, ReturnBillFullInputDto input)
        {
            ZoneCustomerFromToDateDto bedBesGetDto = new(customerInfo.ZoneId, customerInfo.Radif, input.FromDateJalali, input.ToDateJalali);
            IEnumerable<BedBesCreateDto> bedBesInfo = await _bedBesQueryService.Get(bedBesGetDto);
            if (!bedBesInfo.Any())
            {
                throw new ReturnedBillException(ExceptionLiterals.NotFoundBillsToRemoved);
            }

            return bedBesInfo;
        }
        private async Task UpdateBedBesDel(IEnumerable<BedBesCreateDto> bedBes)
        {
            IEnumerable<BedBesUpdateDelDto> bedBesUpdate = bedBes
                    .Select(s => new BedBesUpdateDelDto((int)s.Town, s.Id, true))
                    .ToList();

            await _bedBesCommandService.UpdateDel(bedBesUpdate);
        }
        private RepairCreateDto GetRepairCreateDto(IEnumerable<BedBesCreateDto> bedBes, CustomerInfoOutputDto customerInfo, ReturnBillFullInputDto input)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();

            BedBesCreateDto latestBedbes = bedBes.OrderByDescending(x => x.DateBed).FirstOrDefault();
            BedBesCreateDto finalBedBes = new BedBesCreateDto
            {
                AbonFas = bedBes.Sum(x => x.AbonFas),
                FasBaha = bedBes.Sum(x => x.FasBaha),
                Ztadil = bedBes.Sum(x => x.Ztadil),
                Masraf = bedBes.Sum(x => x.Masraf),
                Shahrdari = bedBes.Sum(x => x.Shahrdari),
                Baha = bedBes.Sum(x => x.Baha),
                AbonAb = bedBes.Sum(x => x.AbonAb),
                Pard = bedBes.Sum(x => x.Pard),
                Jam = bedBes.Sum(x => x.Jam),
                ZaribFasl = bedBes.Sum(x => x.ZaribFasl),
                Jarime = bedBes.Sum(x => x.Jarime),
                AbBaha = bedBes.Sum(x => x.AbBaha),
                Ab10 = bedBes.Sum(x => x.Ab10),
                Ab20 = bedBes.Sum(x => x.Ab20),
                Tafavot = bedBes.Sum(x => x.Tafavot),
                Bodjeh = bedBes.Sum(x => x.Bodjeh),
                Rate = bedBes.Sum(x => x.Rate),
                ZaribD = bedBes.Sum(x => x.ZaribD),
                Avarez = bedBes.Sum(x => x.Avarez)
            };
            decimal previousNumber = bedBes.Min(x => x.PriNo);
            decimal currentNumber = bedBes.Max(x => x.PriNo);

            return new RepairCreateDto()
            {
                Town = customerInfo.ZoneId,
                Radif = customerInfo.Radif,
                Eshtrak = customerInfo.ReadingNumber,
                Barge = 0,
                PriNo = previousNumber,
                TodayNo = currentNumber,
                PriDate = input.FromDateJalali,
                TodayDate = input.ToDateJalali,
                AbonFas = finalBedBes.AbonFas,
                FasBaha = finalBedBes.FasBaha,
                AbBaha = finalBedBes.AbBaha,
                Ztadil = finalBedBes.Ztadil,
                Masraf = finalBedBes.Masraf,
                Shahrdari = finalBedBes.Shahrdari,
                Modat = Duration(input.FromDateJalali, input.ToDateJalali),
                DateBed = currentDateJalali,
                JalaseNo = input.Minutes,
                Mohlat = string.Empty,
                Baha = finalBedBes.Baha,
                AbonAb = finalBedBes.AbonAb,
                Pard = finalBedBes.Pard,
                Jam = finalBedBes.Jam,
                CodVas = 02,
                Ghabs = string.Empty,
                Del = false,
                Type = "4",
                CodEnshab = customerInfo.UsageId,
                Enshab = customerInfo.MeterDiameterId,
                Elat = input.ReturnCauseId,
                Serial = latestBedbes.Serial,
                Ser = 0,
                ZaribFasl = 0,//
                Ab10 = 0,
                Ab20 = 0,
                TedadMas = customerInfo.DomesticUnit,
                TedadTej = customerInfo.CommertialUnit,
                TedadVahd = customerInfo.OtherUnit,
                NoeVa = customerInfo.BranchType,
                Jarime = 0,//
                Masjar = 0,
                Sabt = 0,
                Rate = finalBedBes.Rate,
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
                TedGhabs = bedBes.Count(),
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                Bodjeh = finalBedBes.Bodjeh,
                Group1 = customerInfo.UsageId,
                Faz = customerInfo.SewageCalcState > 0 ? true : false,
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
                Avarez = finalBedBes.Avarez,
                TedKhane = customerInfo.HouseholdNumber
            };
        }
        private AutoBackCreateDto GetAutoBackCreateDto(IEnumerable<BedBesCreateDto> bedBes, RepairCreateDto repair)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            string currentDateJalali10Char = currentDateJalali.Substring(2);

            return new AutoBackCreateDto()
            {
                Town = repair.Town,
                Radif = repair.Radif,
                Eshtrak = repair.Eshtrak,
                Barge = 0,
                PriNo = repair.PriNo,
                TodayNo = repair.TodayNo,
                PriDate = repair.PriDate,
                TodayDate = repair.TodayDate,
                AbonFas = 0,
                FasBaha = 0,
                AbBaha = 0,
                Ztadil = 0,
                Masraf = 0,
                Shahrdari = 0,
                Modat = 0,
                DateBed = currentDateJalali,
                JalaseNo = repair.JalaseNo,
                Mohlat = string.Empty,
                Baha = 0,
                AbonAb = 0,
                Pard = 0,
                Jam = 0,
                CodVas = 0,
                Ghabs = string.Empty,
                Del = false,
                Type = "4",
                CodEnshab = repair.CodEnshab,
                Enshab = repair.Enshab,
                Elat = 0,
                Serial = repair.Serial,
                Ser = 0,
                ZaribFasl = 0,
                Ab10 = 0,
                Ab20 = 0,
                TedadVahd = repair.TedadVahd,
                TedadMas = repair.TedadMas,
                TedadTej = repair.TedadTej,
                TedKhane = repair.TedKhane,
                NoeVa = repair.NoeVa,
                Jarime = 0,
                Masjar = 0,
                Sabt = 0,
                Rate = 0,
                Operator = 0,
                Mamor = 0,
                TavizDate = string.Empty,
                ZaribCntr = 0,
                Zabresani = 0,
                ZaribD = 0,
                Tafavot = 0,
                MasHadar = 0,
                AbHadar = 0,
                RangeMas = 0,
                TafBack = 0,
                TedGhabs = 0,
                TabAbnA = 0,
                TabAbnF = 0,
                TabsFa = 0,
                Bodjeh = 0,
                Faz = false,
                TmpPriDate = string.Empty,
                TmpTodayDate = currentDateJalali10Char,
                TmpMohlat = string.Empty,
                TmpTavizDate = string.Empty,
                TmpDateBed = currentDateJalali10Char,
            };
        }
        private int Duration(string currentDateJalali, string previousDateJalali)
        {
            var previousGregorian = previousDateJalali.ToGregorianDateTime();
            var currentGregorian = currentDateJalali.ToGregorianDateTime();
            int duration = (currentGregorian.Value - previousGregorian.Value).Days;
            if (duration <= 0)
            {
                throw new ReturnedBillException(ExceptionLiterals.CurrentDateNotMoreThanPreviousDate);
            }
            return duration;
        }
        private async Task FromToDateValidation(ReturnBillFullInputDto input, CustomerInfoOutputDto customerInfo)
        {
            int fromCount = await _bedBesQueryService.GetCountInDateBed(customerInfo.ZoneId, customerInfo.Radif, input.FromDateJalali);
            if (fromCount <= 0)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidFromDate);
            }

            int toCount = await _bedBesQueryService.GetCountInDateBed(customerInfo.ZoneId, customerInfo.Radif, input.ToDateJalali);
            if (toCount <= 0)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidToDate);
            }
        }
        private async Task ValidationFullInputDto(ReturnBillFullInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }

    }
}