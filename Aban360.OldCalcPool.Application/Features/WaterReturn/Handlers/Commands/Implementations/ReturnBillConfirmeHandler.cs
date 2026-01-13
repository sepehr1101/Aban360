using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Aban360.OldCalcPool.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Command.Contracts;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPools.WaterReturn.Dto.Queries;
using DNTPersianUtils.Core;
using FluentValidation;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Implementations
{
    internal sealed class ReturnBillConfirmeHandler : IReturnBillConfirmeHandler
    {
        private readonly IRepairCommandService _repairCommandService;
        private readonly IBedBesCommandService _billCommandService;
        private readonly IAutoBackQueryService _autoBackQueryService;
        private readonly IMembersQueryService _membersQueryService;
        private readonly IRepairQueryService _repairQueryService;
        private readonly IValidator<ReturnBillConfirmeByBillIdInputDto> _validator;
        public ReturnBillConfirmeHandler(
            IRepairCommandService repairCommandService,
            IBedBesCommandService billCommandService,
            IAutoBackQueryService autoBackQueryService,
            IMembersQueryService membersQueryService,
            IRepairQueryService repairQueryService,
            IValidator<ReturnBillConfirmeByBillIdInputDto> validator)
        {
            _repairCommandService = repairCommandService;
            _repairCommandService.NotNull(nameof(repairCommandService));

            _billCommandService = billCommandService;
            _billCommandService.NotNull(nameof(billCommandService));

            _autoBackQueryService = autoBackQueryService;
            _autoBackQueryService.NotNull(nameof(autoBackQueryService));

            _membersQueryService = membersQueryService;
            _membersQueryService.NotNull(nameof(membersQueryService));

            _repairQueryService = repairQueryService;
            _repairQueryService.NotNull(nameof(repairQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }
        public async Task<RepairCreateDto> Handle(ReturnBillConfirmeByBillIdInputDto input, CancellationToken cancellationToken)
        {
            await Validation(input, cancellationToken);
            MemberGetDto memberInfo = await _membersQueryService.Get(input.BillId);
            await ReturnValidation(memberInfo, input.JalaseNumber);

            AutoBackGetDto autoBack_difference = await GetDifferenceAutoBack(input, memberInfo);
            RepairCreateDto repairCreate = GetRepairDto(autoBack_difference);
            await _repairCommandService.Create(repairCreate);
            await UpdateBedBesDel(repairCreate);

            return repairCreate;
        }
        private async Task<AutoBackGetDto> GetDifferenceAutoBack(ReturnBillConfirmeByBillIdInputDto input, MemberGetDto memberInfo)
        {
            ReturnBillConfirmeByZoneAndCustomerNumberInputDto returnBillConfirm = new(memberInfo.ZoneId, memberInfo.CustomerNumber, input.JalaseNumber);
            AutoBackGetDto autoBack_difference = await _autoBackQueryService.Get(returnBillConfirm);
            return autoBack_difference;
        }
        private RepairCreateDto GetRepairDto(AutoBackGetDto input)
        {
            return new RepairCreateDto()
            {
                Town = input.Town,
                Radif = input.Radif,
                Eshtrak = input.Eshtrak,
                Barge = input.Barge,
                PriNo = input.PreviousNumber,
                TodayNo = input.CurrentNumber,
                PriDate = input.PreviousDate,
                TodayDate = input.CurrentDate,
                AbonFas = input.AbonFas,
                FasBaha = input.FasBaha,
                AbBaha = input.AbBaha,
                Ztadil = input.Ztadil,
                Masraf = input.Masraf,
                Shahrdari = input.Shahrdari,
                Modat = input.Modat,
                DateBed = input.DateBed,
                JalaseNo = input.JalaseNo,
                Mohlat = input.Mohlat,
                Baha = input.Baha,
                AbonAb = input.AbonAb,
                Pard = input.Pard,
                Jam = input.Jam,
                CodVas = input.CodVas,
                Ghabs = input.Ghabs,
                Del = input.Del,
                Type = input.Type,
                CodEnshab = input.CodEnshab,
                Enshab = input.Enshab,
                Elat = input.Elat,
                Serial = input.Serial,
                Ser = input.Ser,
                ZaribFasl = input.ZaribFasl,
                Ab10 = input.Ab10,
                Ab20 = input.Ab20,
                TedadVahd = input.TedadVahd,
                TedKhane = input.TedKhane,
                TedadMas = input.TedadMas,
                TedadTej = input.TedadTej,
                NoeVa = input.NoeVa,
                Jarime = input.Jarime,
                Masjar = input.Masjar,
                Sabt = input.Sabt,
                Rate = input.Rate,
                Operator = input.Operator,
                Mamor = input.Mamor,
                TavizDate = input.TavizDate,
                ZaribCntr = input.ZaribCntr,
                Zabresani = input.Zabresani,
                ZaribD = input.ZaribD,
                Tafavot = input.Tafavot,
                MasHadar = input.MasHadar,
                AbHadar = input.AbHadar,
                RangeMas = input.RangeMas,
                TafBack = input.TafBack,
                TedGhabs = input.TedGhabs,
                TabAbnA = input.TabAbnA,
                TabAbnF = input.TabAbnF,
                TabsFa = input.TabsFa,
                Bodjeh = input.Bodjeh,
                Faz = input.Faz,
                TmpPriDate = input.TmpPriDate,
                TmpDateBed = input.TmpDateBed,
                TmpMohlat = input.TmpMohlat,
                TmpTavizDate = input.TmpTavizDate,
                TmpTodayDate = input.TmpTodayDate,
                DateSbt = DateTime.Now.ToShortPersianDateString()
            };
        }
        private async Task UpdateBedBesDel(RepairCreateDto input)
        {
            BedBesUpdateDelWithDateDto bedBesUpdate = new((int)input.Town, (int)input.Radif, true, input.PriDate, input.TodayDate);
            await _billCommandService.UpdateDel(bedBesUpdate);
        }
        private async Task Validation(ReturnBillConfirmeByBillIdInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task ReturnValidation(MemberGetDto memberInfo, int jalaseNumber)
        {
            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = new(memberInfo.ZoneId, memberInfo.CustomerNumber);
            int repairCount = await _repairQueryService.GetRepairCount(zoneIdAndCustomerNumber, jalaseNumber);
            if (repairCount > 0)
            {
                throw new ReturnedBillException(ExceptionLiterals.InvalidReturnDuplicate);
            }
        }
    }
}
