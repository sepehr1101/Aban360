using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class InitialCalculationHandler : IInitialCalculationHandler
    {
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IMeterReadingDetailService _meterReadingDetailService;
        private readonly IMeterFlowService _meterFlowService;
        private readonly IOldTariffEngine _tariffEngine;
        public InitialCalculationHandler(
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
            IMeterReadingDetailService meterReadingDetailService,
            IMeterFlowService meterFlowService,
            IOldTariffEngine tariffEngine)
        {

            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));

            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));

            _tariffEngine = tariffEngine;
            _tariffEngine.NotNull(nameof(tariffEngine));
        }

        public async Task<IEnumerable<MeterReadingDetailDataOutputDto>> Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(latestFlowId, cancellationToken);
            //todo: use cancellationToken
            int firstFlowId=await _meterFlowService.GetFirstFlowId(latestFlowId);
            IEnumerable<MeterReadingDetailDataOutputDto> readingDetails = await _meterReadingDetailService.Get(firstFlowId);
            ICollection<MeterReadingWithAbBahaResultUpdateDto> consumptionsInfo = new List<MeterReadingWithAbBahaResultUpdateDto>();
            foreach (var readingDetail in readingDetails)
            {
                if (CounterStateValidation(readingDetail.CurrentCounterStateCode, readingDetail.CurrentNumber, readingDetail.PreviousNumber))
                {
                    MeterImaginaryInputDto meterImaginary = GetMeterImaginary(readingDetail);
                    AbBahaCalculationDetails abBahaCalc = await _tariffEngine.Handle(meterImaginary, cancellationToken);
                    consumptionsInfo.Add(GetMeterReadingDetail(readingDetail, abBahaCalc));

                    readingDetail.SumItems = abBahaCalc.SumItems;
                    readingDetail.SumItemsBeforeDiscount = abBahaCalc.SumItemsBeforeDiscount;
                    readingDetail.DiscountSum = abBahaCalc.DiscountSum;
                    readingDetail.Consumption = abBahaCalc.Consumption;
                    readingDetail.MonthlyConsumption = abBahaCalc.MonthlyConsumption;

                    //remove duplicate code
                }
                else
                {
                    MeterReadingWithAbBahaResultUpdateDto zeroValue = new(readingDetail.Id, 0, 0, 0, 0, 0);
                    consumptionsInfo.Add(zeroValue);

                    readingDetail.SumItems = 0;
                    readingDetail.SumItemsBeforeDiscount = 0;
                    readingDetail.DiscountSum = 0;
                    readingDetail.Consumption = 0;
                    readingDetail.MonthlyConsumption = 0;
                }
            }
            await _meterReadingDetailService.Update(consumptionsInfo);
            await CreateCalculatedFlow(latestFlowId, appUser);

            return readingDetails;
        }
        private async Task CreateCalculatedFlow(int latestFlowId, IAppUser appUser)
        {
            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            _meterFlowService.Update(meterFlowUpdate);

            MeterFlowGetDto meterFlow = await _meterFlowService.Get(latestFlowId);
            MeterFlowCreateDto newMeterFlow = new()
            {
                MeterFlowStepId = MeterFlowStepEnum.Calculated,
                ZoneId = meterFlow.ZoneId,
                FileName = meterFlow.FileName,
                InsertByUserId = appUser.UserId,
                InsertDateTime = DateTime.Now,
            };
            await _meterFlowService.Create(newMeterFlow);
        }
        private MeterImaginaryInputDto GetMeterImaginary(MeterReadingDetailDataOutputDto readingDetail)
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
        private MeterReadingWithAbBahaResultUpdateDto GetMeterReadingDetail(MeterReadingDetailDataOutputDto readingDetail, AbBahaCalculationDetails abBahaCalc)
        {
            return new MeterReadingWithAbBahaResultUpdateDto(
                readingDetail.Id,
                abBahaCalc.SumItems,
                abBahaCalc.SumItemsBeforeDiscount,
                abBahaCalc.DiscountSum,
                abBahaCalc.Consumption,
                abBahaCalc.MonthlyConsumption
                );
        }
        private bool CounterStateValidation(int counterStateCode, int currentNumber, int previousNumber)
        {
            int[] invalidCounterStateCode = new int[] { 4, 6, 7, 8, 9, 10 };

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
