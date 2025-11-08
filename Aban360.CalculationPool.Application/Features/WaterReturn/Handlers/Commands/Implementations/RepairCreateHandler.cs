using Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.WaterReturn.Command.Contracts;
using Aban360.CalculationPool.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class RepairCreateHandler : IRepairCreateHandler
    {
        private readonly IRepairCommandService _commandService;
        private readonly IMembersQueryService _membersQueryService;
        private readonly IValidator<OfferingToCreateRepairDto> _validator;
        public RepairCreateHandler(
            IRepairCommandService commandService,
            IMembersQueryService membersQueryService,
            IValidator<OfferingToCreateRepairDto> validator)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _membersQueryService = membersQueryService;
            _membersQueryService.NotNull(nameof(membersQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }
        //ToDo: OfferingToCreateRepairDto Validation
        public async Task Handle(OfferingToCreateRepairDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            MemberGetDto memberInfo = await _membersQueryService.Get(createDto.BillId);
            RepairCreateDto repairDto = GetRepairCreateDto(createDto, memberInfo);
            await _commandService.Create(repairDto);
        }
        private RepairCreateDto GetRepairCreateDto(OfferingToCreateRepairDto offeringInfo, MemberGetDto memberInfo)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
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
                Mohlat = string.Empty,
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
    }
}
