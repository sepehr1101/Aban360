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

            IEnumerable<BedBesCreateDto> bedBesInfo = await GetBedBesList(customerInfo, input);

            await UpdateBedBesDel(bedBesInfo);
            RepairCreateDto repairCreate = GetRepairCreateDto(bedBesInfo, customerInfo, input);
            AutoBackCreateDto autoBackCreate = GetAutoBackCreateDto(bedBesInfo, repairCreate);
            if (!input.IsConfirm)
            {
                return new ReturnBillOutputDto(bedBesInfo, repairCreate, autoBackCreate);

            }

            await _repairCommandService.Create(repairCreate);//todo : remove comment
            await _autoBackCommandService.Create(autoBackCreate);

            return new ReturnBillOutputDto(bedBesInfo, repairCreate, autoBackCreate);
        }
        private async Task<IEnumerable<BedBesCreateDto>> GetBedBesList(CustomerInfoOutputDto customerInfo, ReturnBillFullInputDto input)
        {
            ZoneCustomerFromToDateDto bedBesGetDto = new(customerInfo.ZoneId, customerInfo.Radif, input.FromDateJalali, input.ToDateJalali);
            IEnumerable<BedBesCreateDto> bedBesInfo = await _bedBesQueryService.Get(bedBesGetDto);

            return bedBesInfo.Any() ? bedBesInfo : throw new ReturnedBillException(ExceptionLiterals.NotFoundBillsToRemoved);
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
            BedBesCreateDto latestBedbes = bedBes.OrderByDescending(x => x.DateBed).FirstOrDefault();
            BedBesCreateDto finalBedBes = bedBes.Aggregate(new BedBesCreateDto(), (a, b) =>
            {
                a.AbonFas += b.AbonFas;
                a.FasBaha += b.FasBaha;
                a.Ztadil += b.Ztadil;
                a.Masraf += b.Masraf;
                a.Shahrdari += b.Shahrdari;
                a.Baha += b.Baha;
                a.AbonAb += b.AbonAb;
                a.Pard += b.Pard;
                a.Jam += b.Jam;
                a.ZaribFasl += b.ZaribFasl;
                a.Jarime += b.Jarime;
                a.AbBaha += b.AbBaha;
                a.Ab10 += b.Ab10;
                a.Ab20 += b.Ab20;
                a.Tafavot += b.Tafavot;
                a.Bodjeh += b.Bodjeh;
                a.Rate += b.Rate;
                a.ZaribD += b.ZaribD;
                a.Avarez += b.Avarez;

                return a;
            });

            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            decimal previousNumber = bedBes.Min(x => x.PriNo);
            decimal currentNumber = bedBes.Max(x => x.TodayNo);
            int duration = Duration(input.ToDateJalali, input.FromDateJalali);

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
                Modat = duration,
                DateBed = currentDateJalali,
                JalaseNo = input.Minutes,
                Mohlat = string.Empty,
                Baha = finalBedBes.Baha,
                AbonAb = finalBedBes.AbonAb,
                Pard = (finalBedBes.Pard / 1000) * 1000,
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
                EdarehK = customerInfo.IsSpecial,
                Lavazem = 0,//
                DateSbt = currentDateJalali,
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

            return duration > 0 ? duration : throw new ReturnedBillException(ExceptionLiterals.CurrentDateNotMoreThanPreviousDate);
        }
        private async Task FromToDateValidation(ReturnBillFullInputDto input, CustomerInfoOutputDto customerInfo)
        {
            async Task Validation(string date, string exceptionMessage)
            {
                int count = await _bedBesQueryService.GetCountInDateBed(customerInfo.ZoneId, customerInfo.Radif, date);
                _ = count <= 0 ? throw new ReturnedBillException(exceptionMessage) : 0;
            }

            await Validation(input.FromDateJalali, ExceptionLiterals.InvalidFromDate);
            await Validation(input.ToDateJalali, ExceptionLiterals.InvalidToDate);
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