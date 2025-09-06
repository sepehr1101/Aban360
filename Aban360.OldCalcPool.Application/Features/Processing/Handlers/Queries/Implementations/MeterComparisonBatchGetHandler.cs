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
    internal sealed class MeterComparisonBatchGetHandler : IMeterComparisonBatchGetHandler
    {
        private readonly IProcessing _processing;
        private readonly IMeterComparisonBatchQueryService _meterComparisonBatchQueryService;
        private readonly IValidator<MeterComparisonBatchInputDto> _validator;
        float _percent = (float)0.08;
        public MeterComparisonBatchGetHandler(
            IProcessing processing,
            IMeterComparisonBatchQueryService meterComparisonBatchQueryService,
            IValidator<MeterComparisonBatchInputDto> validator)
        {
            _processing = processing;
            _processing.NotNull(nameof(processing));

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
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataWithCustomerInfoOutputDto> meterComparisonBatch = await _meterComparisonBatchQueryService.Get(input);
            ICollection<MeterComparisonBatchDataOutputDto> comparisonResult = new List<MeterComparisonBatchDataOutputDto>();
            foreach (var data in meterComparisonBatch.ReportData)
            {
                BaseOldTariffEngineImaginaryInputDto meterInfoData = GetMeterInfo(data);
                var result = await _processing.Handle(meterInfoData, cancellationToken);

                MeterComparisonBatchDataOutputDto comparisonBatch = GetComparisonBatch(data);
                comparisonBatch.IsChecked = GetTolarance(data.PreviousAmount, data.CurrentAmount, input.Tolerance, input.IsPercent);
                // comparisonBatch.CurrentAmount = comparisonBatch.IsChecked ? comparisonBatch.PreviousAmount : comparisonBatch.CurrentAmount;

                comparisonResult.Add(comparisonBatch);
            }
            meterComparisonBatch.ReportHeader.SumCurrentAmount = comparisonResult.Sum(m => m.CurrentAmount);


            ReportOutput<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataOutputDto> meterComparisonResult = new(meterComparisonBatch.Title, meterComparisonBatch.ReportHeader, comparisonResult);
            return meterComparisonResult;
        }

        private MeterComparisonBatchDataOutputDto GetComparisonBatch(MeterComparisonBatchDataWithCustomerInfoOutputDto data)
        {
            return new MeterComparisonBatchDataOutputDto()
            {
                BillId = data.BillId,
                PreviousDateJalali = data.PreviousDateJalali,
                CurrentDateJalali = data.CurrentDateJalali,
                CurrentMeterNumber = data.CurrentMeterNumber,
                PreviousMeterNumber = data.PreviousMeterNumber,
                PreviousAmount = data.PreviousAmount,
                ZoneTitle = data.ZoneTitle,
            };
        }
        private BaseOldTariffEngineImaginaryInputDto GetMeterInfo(MeterComparisonBatchDataWithCustomerInfoOutputDto data)
        {
            BaseOldTariffEngineImaginaryInputDto meterInfoData = new()
            {
                MeterPreviousData = new MeterInfoByPreviousDataInputDto()
                {
                    BillId = data.BillId,
                    CurrentDateJalali = data.CurrentDateJalali,
                    CurrentMeterNumber = data.CurrentMeterNumber,
                    PreviousDateJalali = data.PreviousDateJalali,
                    PreviousNumber = data.PreviousMeterNumber
                },
                customerInfo = new CustomerDetailInfoInputDto()
                {
                    ZoneId = data.ZoneId,
                    Radif = data.Radif,
                    BranchType = data.BranchType,
                    UsageId = data.UsageId,
                    DomesticUnit = data.DomesticUnit,
                    CommertialUnit = data.CommertialUnit,
                    OtherUnit = data.OtherUnit,
                    WaterInstallationDateJalali = data.WaterInstallationDateJalali,
                    SewageInstallationDateJalali = data.SewageInstallationDateJalali,
                    WaterCount = data.WaterCount,
                    SewageCalcState = data.SewageCalcState,
                    ContractualCapacity = data.ContractualCapacity,
                    HouseholdNumber = data.HouseholdNumber,
                    ReadingNumber = data.ReadingNumber,
                    VillageId = data.VillageId,
                    IsSpecial = data.IsSpecial,

                }
            };
            return meterInfoData;
        }
        private bool GetTolarance(double previousAmount, double currentAmount, double tolerance, bool isPercent)
        {
            if (isPercent)
            {
                var (maxAmount,minAmount)=GetMaxMinPercent(currentAmount, tolerance);
                return currentAmount <= maxAmount && currentAmount >= minAmount;
            }

            return Math.Abs(previousAmount - currentAmount) <= tolerance;
        }
        private (double, double) GetMaxMinPercent(double amount, double tolerance)
        {
            return (amount * (1 + tolerance), amount * (1 - tolerance));
        }
    }
}
