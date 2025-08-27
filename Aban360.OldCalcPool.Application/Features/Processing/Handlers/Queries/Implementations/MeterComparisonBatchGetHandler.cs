using Aban360.Common.Excel;
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

            ReportOutput<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataOutputDto> meterComparisonBatch = await _meterComparisonBatchQueryService.Get(input);
            foreach (var data in meterComparisonBatch.ReportData)
            {
                MeterInfoByPreviousDataInputDto meterInfoByPreviousData = new()
                {
                    BillId = data.BillId,
                    CurrentDateJalali = data.CurrentDateJalali,
                    CurrentMeterNumber = data.CurrentMeterNumber,
                    PreviousDateJalali = data.PreviousDateJalali,
                    PreviousNumber = data.PreviousMeterNumber
                };
                var result = await _processing.Handle(meterInfoByPreviousData, cancellationToken);
                data.CurrentAmount = result.SumItems;
                data.IsChecked = GetTolarance(data.PreviousAmount,data.CurrentAmount,input.Tolerance);
            }
            meterComparisonBatch.ReportHeader.SumCurrentAmount = meterComparisonBatch.ReportData.Sum(m => m.CurrentAmount);
            return meterComparisonBatch;
        }
        private bool GetTolarance(double previousAmount, double currentAmount,double tolerance)
        {
            return Math.Abs(previousAmount - currentAmount) <= tolerance ? true : false;
        }
    }
}
