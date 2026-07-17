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
    internal sealed class MeterReadingDetailExcludedGetHadler : IMeterReadingDetailExcludedGetHadler
    {
        private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IValidator<MeterReadingDetailExcludedInputDto> _validator;
        private string _title = ReportLiterals.MeterReadingExcluded;
        public MeterReadingDetailExcludedGetHadler(
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
            ICommonZoneService commonZoneService,
            IValidator<MeterReadingDetailExcludedInputDto> validator)
        {
            _meterReadingDetailQueryService = meterReadingDetailQueryService;
            _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(validator));

            _validator = validator;
            _validator.NotNull(nameof(meterReadingDetailQueryService));
        }

        public async Task<ReportOutput<MeterReadingDetailExcludedHeaderOutptuDto, MeterReadingDetailExcludedDataOutputDto>> Handle(MeterReadingDetailExcludedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, appUser, cancellationToken);
            IEnumerable<MeterReadingDetailExcludedDataOutputDto> data = await _meterReadingDetailQueryService.Get(inputDto);
            MeterReadingDetailExcludedHeaderOutptuDto header = new()
            {
                RecordCount = data?.Count() ?? 0,
                Title = _title
            };

            return new ReportOutput<MeterReadingDetailExcludedHeaderOutptuDto, MeterReadingDetailExcludedDataOutputDto>(_title, header, data);
        }
        private async Task Validate(MeterReadingDetailExcludedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
        }
        private async Task InputValidate(MeterReadingDetailExcludedInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
    }
}
