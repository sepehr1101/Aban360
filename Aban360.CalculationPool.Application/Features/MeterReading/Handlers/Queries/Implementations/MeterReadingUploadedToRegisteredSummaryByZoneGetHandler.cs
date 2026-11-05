using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterReadingUploadedToRegisteredSummaryByZoneGetHandler : IMeterReadingUploadedToRegisteredSummaryByZoneGetHandler
    {
        private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IValidator<MeterReadingUploadToRegisteredInputDto> _validator;
        private static string _title = ReportLiterals.MeterReadingUploadedToRegistered;
        public MeterReadingUploadedToRegisteredSummaryByZoneGetHandler(
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
            ICommonZoneService commonZoneService,
            IValidator<MeterReadingUploadToRegisteredInputDto> validator)
        {
            _meterReadingDetailQueryService = meterReadingDetailQueryService;
            _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(validator));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MeterReadingUploadToRegisteredHeaderOutputDto, MeterReadingUploadToRegisteredSummaryDataOutputDto>> Handle(MeterReadingUploadToRegisteredInputDto inputDto, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto,cancellationToken);
            IEnumerable<MeterReadingUploadToRegisteredSummaryDataOutputDto> data = await _meterReadingDetailQueryService.Get(inputDto);
            MeterReadingUploadToRegisteredHeaderOutputDto header = new()
            {
                FromDateJalali = inputDto.FromDateJalali,
                ToDateJalali = inputDto.ToDateJalali,

                FileCount = data?.Sum(d => d.FileCount) ?? 0,
                AllRecords = data?.Sum(d => d.AllRecords) ?? 0,
                CustomerCount = data?.Sum(d => d.CustomerCount) ?? 0,
                RemovedAndUpdatedCount = data?.Sum(d => d.RemovedAndUpdatedCount) ?? 0,
                ExcludedCount = data?.Sum(d => d.ExcludedCount) ?? 0,
                DistanceMax = data?.Max(d => d.DistanceMax) ?? 0,
                DistanceMin = data?.Min(d => d.DistanceMin) ?? 0,
                DistanceAverage = data?.Average(d => d.DistanceAverage) ?? 0,
                DistanceMedian = data?.Average(d => (float)d.DistanceMedian) ?? 0,

                Title = _title
            };
            ReportOutput<MeterReadingUploadToRegisteredHeaderOutputDto, MeterReadingUploadToRegisteredSummaryDataOutputDto> result = new(_title, header, data);
            return result;
        }
        public async Task InputValidate(MeterReadingUploadToRegisteredInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                string message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
