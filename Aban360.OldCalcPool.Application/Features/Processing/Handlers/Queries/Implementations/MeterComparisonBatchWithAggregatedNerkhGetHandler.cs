using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Implementations
{
    internal sealed class MeterComparisonBatchWithAggregatedNerkhGetHandler : IMeterComparisonBatchWithAggregatedNerkhGetHandler
    {
        private readonly IOldTariffEngine _oldCalcEngine;
        private readonly IMeterComparisonBatchQueryService _meterComparisonBatchQueryService;
        private readonly IValidator<MeterComparisonBatchInputDto> _validator;

        public MeterComparisonBatchWithAggregatedNerkhGetHandler(
            IOldTariffEngine processing,
            IMeterComparisonBatchQueryService meterComparisonBatchQueryService,
            IValidator<MeterComparisonBatchInputDto> validator)
        {
            _oldCalcEngine = processing;
            _oldCalcEngine.NotNull(nameof(processing));

            _meterComparisonBatchQueryService = meterComparisonBatchQueryService;
            _meterComparisonBatchQueryService.NotNull(nameof(meterComparisonBatchQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }
        public async Task<ReportOutput<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataOutputDto>> Handle(MeterComparisonBatchInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                string message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataWithCustomerInfoOutputDto> meterComparisonBatch = await _meterComparisonBatchQueryService.Get(input);
            ICollection<MeterComparisonBatchDataOutputDto> comparisonResult = new List<MeterComparisonBatchDataOutputDto>();
            foreach (var data in meterComparisonBatch.ReportData)
            {
                MeterImaginaryInputDto meterInfoData = CreateImaginaryInputDtoObject(data);
                AbBahaCalculationDetails result = await _oldCalcEngine.Handle(meterInfoData, cancellationToken);

                MeterComparisonBatchDataOutputDto comparisonBatch = CreateComparisonBatchObject(data);
                comparisonBatch.CurrentAmount = result.SumItems;
                comparisonBatch.IsChecked = GetTolarance(data.PreviousAmount, result.SumItems, input.Tolerance, input.IsPercent);
                comparisonBatch.CurrentDiscountAmount = result.DiscountSum;
                comparisonBatch.ComparisonAmount = GetComparison(data.PreviousAmount, result.SumItems);
                comparisonResult.Add(comparisonBatch);
            }

            meterComparisonBatch.ReportHeader.SumCurrentAmount = comparisonResult.Sum(m => m.CurrentAmount);
            meterComparisonBatch.ReportHeader.InvalidCount = comparisonResult.Count(m => !m.IsChecked);
            meterComparisonBatch.ReportHeader.ValidCount = comparisonResult.Count(m => m.IsChecked);
            meterComparisonBatch.ReportHeader.DifferenceSum = comparisonResult.Sum(m => m.ComparisonAmount);

            ReportOutput<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataOutputDto> meterComparisonResult = new(meterComparisonBatch.Title, meterComparisonBatch.ReportHeader, comparisonResult);
            return meterComparisonResult;
        }

        private MeterComparisonBatchDataOutputDto CreateComparisonBatchObject(MeterComparisonBatchDataWithCustomerInfoOutputDto data)
        {
            return new MeterComparisonBatchDataOutputDto()
            {
                BillId = data.BillId,
                PreviousDateJalali = data.PreviousDateJalali,
                CurrentDateJalali = data.CurrentDateJalali,
                PreviousMeterNumber = data.PreviousMeterNumber,
                CurrentMeterNumber = data.CurrentMeterNumber,
                PreviousAmount = data.PreviousAmount,
                PreviousDiscountAmount = data.PreviousDiscount,
                ZoneTitle = data.ZoneTitle,
                UsageId = data.UsageId,
                BranchId = data.BranchType,
                DomesticUnit = data.DomesticUnit,
                CommercialUnit = data.CommertialUnit,
                OtherUnit = data.OtherUnit,
                EmptyUnit = data.EmptyUnit,
                ContractualCapacity = data.ContractualCapacity,
                VirtualCategoryId = data.VirtualCategoryId,
            };
        }
        private MeterImaginaryInputDto CreateImaginaryInputDtoObject(MeterComparisonBatchDataWithCustomerInfoOutputDto data)
        {
            MeterImaginaryInputDto meterInfoData = new()
            {
                MeterPreviousData = new MeterInfoByPreviousDataInputDto()
                {
                    BillId = data.BillId,
                    CurrentDateJalali = data.CurrentDateJalali,
                    CurrentMeterNumber = data.CurrentMeterNumber,
                    PreviousDateJalali = data.PreviousDateJalali,
                    PreviousNumber = data.PreviousMeterNumber
                },
                CustomerInfo = new CustomerDetailInfoInputDto()
                {
                    ZoneId = data.ZoneId,
                    Radif = data.Radif,
                    BranchType = data.BranchType,
                    UsageId = data.UsageId,
                    DomesticUnit = data.DomesticUnit,
                    CommertialUnit = data.CommertialUnit,
                    OtherUnit = data.OtherUnit,
                    EmptyUnit = data.EmptyUnit,
                    WaterInstallationDateJalali = data.WaterInstallationDateJalali,
                    SewageInstallationDateJalali = data.SewageInstallationDateJalali,
                    WaterRegisterDate = data.WaterRegisterDate,
                    SewageRegisterDate = data.SewageRegisterDate,
                    //WaterCount = data.WaterCount,
                    SewageCalcState = data.SewageCalcState,
                    ContractualCapacity = data.ContractualCapacity,
                    HouseholdNumber = data.HouseholdNumber,
                    HouseholdDate = data.HouseholdDate,
                    ReadingNumber = data.ReadingNumber,
                    VillageId = data.VillageId,
                    IsSpecial = data.IsSpecial,
                    VirtualCategoryId = data.VirtualCategoryId,
                }
            };
            return meterInfoData;
        }
        private bool GetTolarance(double previousAmount, double currentAmount, double tolerance, bool isPercent)
        {
            if (isPercent)
            {
                var (maxAmount, minAmount) = GetMaxMinPercent(previousAmount, tolerance);
                return currentAmount <= maxAmount && currentAmount >= minAmount;
            }

            return Math.Abs(previousAmount - currentAmount) <= tolerance;
        }
        private (double, double) GetMaxMinPercent(double amount, double tolerance)
        {
            return (amount * (1 + tolerance / 100), amount * (1 - tolerance / 100));
        }
        private double GetComparison(double firstAmount, double secondAmount)
        {
            return Math.Abs(firstAmount - secondAmount);
        }
    }
}
