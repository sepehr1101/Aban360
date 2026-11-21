using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Services;
public interface IMeterReadingFileReportsService
{
    Task<ReportOutput<MeterReadingExcelFileDownloadHeaderOutputDto, MeterReadingExcelFileDownloadDateOutputDto>> MeterReadingDownloadExcelFileAsync(MeterReadingExcelFileDownloadDto input, CancellationToken cancellationToken);
    Task<ReportOutput<MeterReadingUploadToRegisteredHeaderOutputDto, MeterReadingUploadToRegisteredSummaryDataOutputDto>> MeterReadingUploadedToRegisteredSummaryByZoneGetAsync(MeterReadingUploadToRegisteredInputDto inputDto, CancellationToken cancellationToken);
    Task MeterReadingUploadedToRegisteredSummaryByZoneGetInputValidate(MeterReadingUploadToRegisteredInputDto inputDto, CancellationToken cancellationToken);
}

internal sealed class MeterReadingFileReportsService : IMeterReadingFileReportsService
{
    private readonly IBillQueryService _billQueryService;
    private readonly ICommonZoneService _commonZoneService;
    private string _title = ReportLiterals.WaterLatestList;
    private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
    private readonly IValidator<MeterReadingUploadToRegisteredInputDto> _validator;
    private static string _meterReadingUploadedToRegisteredSummaryByZoneGettitle = ReportLiterals.MeterReadingUploadedToRegistered;
    public MeterReadingFileReportsService(IBillQueryService billQueryService, ICommonZoneService commonZoneService, IMeterReadingDetailQueryService meterReadingDetailQueryService, IValidator<MeterReadingUploadToRegisteredInputDto> validator)
    {
        _billQueryService = billQueryService;
        _billQueryService.NotNull(nameof(billQueryService));
        _commonZoneService = commonZoneService;
        _commonZoneService.NotNull(nameof(commonZoneService));
        _meterReadingDetailQueryService = meterReadingDetailQueryService;
        _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));
        _commonZoneService.NotNull(nameof(validator));
        _validator = validator;
        _validator.NotNull(nameof(validator));
    }

    public async Task<ReportOutput<MeterReadingExcelFileDownloadHeaderOutputDto, MeterReadingExcelFileDownloadDateOutputDto>> MeterReadingDownloadExcelFileAsync(MeterReadingExcelFileDownloadDto input, CancellationToken cancellationToken)
    {
        IEnumerable<MeterReadingExcelFileDownloadDateOutputDto> data = await _billQueryService.GetLatestListForExcel(new BillLatestListInputDto(input.ZoneId, input.FromReadingNumber, input.ToReadingNumber));
        MeterReadingExcelFileDownloadHeaderOutputDto header = new()
        {
            ZoneId = input.ZoneId,
            FromReadingNumber = input.FromReadingNumber,
            ToReadingNumber = input.ToReadingNumber,
            RecordCount = data?.Count() ?? 0,
            CustomerCount = data?.Count() ?? 0,
            Title = _title,
        };
        if ((data?.Count() ?? 0) <= 0)
        {
            throw new ReadingException(ExceptionLiterals.NotFoundAnyData);
        }

        return new ReportOutput<MeterReadingExcelFileDownloadHeaderOutputDto, MeterReadingExcelFileDownloadDateOutputDto>(_title, header, data);
    }

    public async Task<ReportOutput<MeterReadingUploadToRegisteredHeaderOutputDto, MeterReadingUploadToRegisteredSummaryDataOutputDto>> MeterReadingUploadedToRegisteredSummaryByZoneGetAsync(MeterReadingUploadToRegisteredInputDto inputDto, CancellationToken cancellationToken)
    {
        await MeterReadingUploadedToRegisteredSummaryByZoneGetInputValidate(inputDto, cancellationToken);
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
            Title = _meterReadingUploadedToRegisteredSummaryByZoneGettitle
        };
        ReportOutput<MeterReadingUploadToRegisteredHeaderOutputDto, MeterReadingUploadToRegisteredSummaryDataOutputDto> result = new(_meterReadingUploadedToRegisteredSummaryByZoneGettitle, header, data);
        return result;
    }

    public async Task MeterReadingUploadedToRegisteredSummaryByZoneGetInputValidate(MeterReadingUploadToRegisteredInputDto inputDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            string message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new CustomValidationException(message);
        }
    }
}
