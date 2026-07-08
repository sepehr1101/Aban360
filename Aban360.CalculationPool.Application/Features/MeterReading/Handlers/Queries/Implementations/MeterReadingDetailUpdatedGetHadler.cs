using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterReadingDetailUpdatedGetHadler : IMeterReadingDetailUpdatedGetHadler
    {
        private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IValidator<MeterReadingDetailUpdatedInputDto> _validator;
        private string _title = ReportLiterals.MeterReadingUpdated;
        public MeterReadingDetailUpdatedGetHadler(
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
            ICommonZoneService commonZoneService,
            IValidator<MeterReadingDetailUpdatedInputDto> validator)
        {
            _meterReadingDetailQueryService = meterReadingDetailQueryService;
            _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(validator));

            _validator = validator;
            _validator.NotNull(nameof(meterReadingDetailQueryService));
        }

        public async Task<ReportOutput<MeterReadingDetailUpdatedHeaderOutptuDto, MeterReadingDetailUpdatedDataOutputDto>> Handle(MeterReadingDetailUpdatedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, appUser, cancellationToken);
            IEnumerable<MeterReadingDetailUpdatedDataOutputDto> data = await _meterReadingDetailQueryService.GetUpdated(inputDto);
            IEnumerable<MeterReadingDetailUpdatedDataOutputDto> finalyData = SetColour(data);
            MeterReadingDetailUpdatedHeaderOutptuDto header = new(finalyData?.Count() ?? 0, _title);

            return new ReportOutput<MeterReadingDetailUpdatedHeaderOutptuDto, MeterReadingDetailUpdatedDataOutputDto>(_title, header, finalyData);
        }
        private IEnumerable<MeterReadingDetailUpdatedDataOutputDto> SetColour(IEnumerable<MeterReadingDetailUpdatedDataOutputDto> data)
        {
            bool isColour = false;
            string? tempBillId = null;
            foreach (var item in data)
            {
                if (tempBillId is not null && item.BillId != tempBillId)
                    isColour = !isColour;

                item.IsColour = isColour;
                tempBillId = item.BillId;
            }

            return data;
        }
        private async Task Validate(MeterReadingDetailUpdatedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
        }
        private async Task InputValidate(MeterReadingDetailUpdatedInputDto inputDto, CancellationToken cancellationToken)
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