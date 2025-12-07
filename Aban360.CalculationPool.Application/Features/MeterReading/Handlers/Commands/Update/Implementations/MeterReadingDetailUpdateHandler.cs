using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Implementations
{
    internal sealed class MeterReadingDetailUpdateHandler : IMeterReadingDetailUpdateHandler
    {
        private readonly IMeterReadingDetailService _meterReadingDetailService;
        private readonly IOldTariffEngine _oldTariffEngine;
        private readonly IValidator<MeterReadingDetailUpdateDto> _validator;
        public MeterReadingDetailUpdateHandler(
             IMeterReadingDetailService meterReadingDetailService,
             IOldTariffEngine oldTariffEngine,
             IValidator<MeterReadingDetailUpdateDto> validator)
        {
            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _oldTariffEngine = oldTariffEngine;
            _oldTariffEngine.NotNull(nameof(oldTariffEngine));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }

        public async Task Handle(MeterReadingDetailUpdateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(input, cancellationToken);
            AbBahaCalculationDetails abBahaResult = await CalcAbBahaTariff(input, cancellationToken);

            MeterReadingDetailCreateDuplicateDto readingCreateDuplicate = new(input.Id, input.CurrentCounterStateCode, input.CurrentDateJalali, input.CurrentNumber, appUser.UserId, DateTime.Now,abBahaResult.SumItems,abBahaResult.SumItemsBeforeDiscount,abBahaResult.DiscountSum,abBahaResult.Consumption,abBahaResult.MonthlyConsumption);
            await _meterReadingDetailService.CreateDuplicateForLog(readingCreateDuplicate);

            //remove previous
            MeterReadingDetailDeleteDto readingDelete = new(input.Id, appUser.UserId, DateTime.Now);
            await _meterReadingDetailService.Delete(readingDelete);
        }

        private async Task Validate(MeterReadingDetailUpdateDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task<AbBahaCalculationDetails> CalcAbBahaTariff(MeterReadingDetailUpdateDto meterReadingDetailUpdate, CancellationToken cancellationToken)
        {
            MeterReadingDetailDataOutputDto meterReadingDetail = await _meterReadingDetailService.GetById(meterReadingDetailUpdate.Id);

            MeterImaginaryInputDto meterImaginary = GetMeterImaginary(meterReadingDetail, meterReadingDetailUpdate);
            AbBahaCalculationDetails abBaha= await _oldTariffEngine.Handle(meterImaginary, cancellationToken);

            return abBaha;
        }
        private MeterImaginaryInputDto GetMeterImaginary(MeterReadingDetailDataOutputDto readingDetail, MeterReadingDetailUpdateDto meterReadingDetailUpdate)
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
                CounterStateCode = meterReadingDetailUpdate.CurrentCounterStateCode,
            };
            MeterInfoByPreviousDataInputDto meterInfo = new()
            {
                BillId = readingDetail.BillId,
                PreviousDateJalali = readingDetail.PreviousDateJalali,
                PreviousNumber = readingDetail.PreviousNumber,
                CurrentDateJalali = meterReadingDetailUpdate.CurrentDateJalali,
                CurrentMeterNumber = meterReadingDetailUpdate.CurrentNumber,
                CounterStateCode = meterReadingDetailUpdate.CurrentCounterStateCode
            };
            return new MeterImaginaryInputDto()
            {
                CustomerInfo = customerInfo,
                MeterPreviousData = meterInfo,
            };
        }

    }
}