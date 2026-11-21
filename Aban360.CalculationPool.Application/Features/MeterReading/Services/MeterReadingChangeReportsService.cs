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

namespace Aban360.CalculationPool.Application.Features.MeterReading.Services;
public interface IMeterReadingChangeReportsService
{
    Task<ReportOutput<MeterReadingDetailExcludedHeaderOutptuDto, MeterReadingDetailExcludedDataOutputDto>> MeterReadingDetailExcludedGetAsync(MeterReadingDetailExcludedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    Task<ReportOutput<MeterReadingDetailUpdatedHeaderOutptuDto, MeterReadingDetailUpdatedDataOutputDto>> MeterReadingDetailUpdatedGetAsync(MeterReadingDetailUpdatedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
}

internal sealed class MeterReadingChangeReportsService : IMeterReadingChangeReportsService
{
    private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
    private readonly ICommonZoneService _commonZoneService;
    private readonly IValidator<MeterReadingDetailExcludedInputDto> _validator;
    private string _title = ReportLiterals.MeterReadingExcluded;
    private readonly IValidator<MeterReadingDetailUpdatedInputDto> _meterReadingDetailUpdatedGetValidator;
    private string _meterReadingDetailUpdatedGetTitle = ReportLiterals.MeterReadingUpdated;
    public MeterReadingChangeReportsService(IMeterReadingDetailQueryService meterReadingDetailQueryService, ICommonZoneService commonZoneService, IValidator<MeterReadingDetailExcludedInputDto> validator, IValidator<MeterReadingDetailUpdatedInputDto> meterReadingDetailUpdatedGetValidator)
    {
        _meterReadingDetailQueryService = meterReadingDetailQueryService;
        _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));
        _commonZoneService = commonZoneService;
        _commonZoneService.NotNull(nameof(validator));
        _validator = validator;
        _validator.NotNull(nameof(meterReadingDetailQueryService));
        _commonZoneService.NotNull(nameof(meterReadingDetailUpdatedGetValidator));
        _meterReadingDetailUpdatedGetValidator = meterReadingDetailUpdatedGetValidator;
        _meterReadingDetailUpdatedGetValidator.NotNull(nameof(meterReadingDetailQueryService));
    }

    public async Task<ReportOutput<MeterReadingDetailExcludedHeaderOutptuDto, MeterReadingDetailExcludedDataOutputDto>> MeterReadingDetailExcludedGetAsync(MeterReadingDetailExcludedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await MeterReadingDetailExcludedGetValidate(inputDto, appUser, cancellationToken);
        IEnumerable<MeterReadingDetailExcludedDataOutputDto> data = await _meterReadingDetailQueryService.Get(inputDto);
        MeterReadingDetailExcludedHeaderOutptuDto header = new()
        {
            FromExcludeDateJalali = inputDto.FromExcludeDateJalali,
            ToExcludeDateJalali = inputDto.ToExcludeDateJalali,
            FromReadingNumber = inputDto.FromReadingNumber,
            ToReadingNumber = inputDto.ToReadingNumber,
            RecordCount = data?.Count() ?? 0,
            CustomerCount = data?.Count() ?? 0,
            Title = _title
        };
        return new ReportOutput<MeterReadingDetailExcludedHeaderOutptuDto, MeterReadingDetailExcludedDataOutputDto>(_title, header, data);
    }

    private async Task MeterReadingDetailExcludedGetValidate(MeterReadingDetailExcludedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await MeterReadingDetailExcludedGetInputValidate(inputDto, cancellationToken);
        await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
    }

    private async Task MeterReadingDetailExcludedGetInputValidate(MeterReadingDetailExcludedInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }

    public async Task<ReportOutput<MeterReadingDetailUpdatedHeaderOutptuDto, MeterReadingDetailUpdatedDataOutputDto>> MeterReadingDetailUpdatedGetAsync(MeterReadingDetailUpdatedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await MeterReadingDetailUpdatedGetValidate(inputDto, appUser, cancellationToken);
        IEnumerable<MeterReadingDetailUpdatedDataOutputDto> data = await _meterReadingDetailQueryService.GetUpdated(inputDto);
        IEnumerable<MeterReadingDetailUpdatedDataOutputDto> finalyData = MeterReadingDetailUpdatedGetSetColour(data);
        MeterReadingDetailUpdatedHeaderOutptuDto header = new(finalyData?.Count() ?? 0, _meterReadingDetailUpdatedGetTitle);
        return new ReportOutput<MeterReadingDetailUpdatedHeaderOutptuDto, MeterReadingDetailUpdatedDataOutputDto>(_meterReadingDetailUpdatedGetTitle, header, finalyData);
    }

    private IEnumerable<MeterReadingDetailUpdatedDataOutputDto> MeterReadingDetailUpdatedGetSetColour(IEnumerable<MeterReadingDetailUpdatedDataOutputDto> data)
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

    private async Task MeterReadingDetailUpdatedGetValidate(MeterReadingDetailUpdatedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
    {
        await MeterReadingDetailUpdatedGetInputValidate(inputDto, cancellationToken);
        await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
    }

    private async Task MeterReadingDetailUpdatedGetInputValidate(MeterReadingDetailUpdatedInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _meterReadingDetailUpdatedGetValidator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }
    }
}
