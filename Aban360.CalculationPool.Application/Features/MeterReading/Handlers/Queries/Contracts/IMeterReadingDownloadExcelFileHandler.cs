using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface IMeterReadingDownloadExcelFileHandler
    {
        Task<ReportOutput<MeterReadingExcelFileDownloadHeaderOutputDto, MeterReadingExcelFileDownloadDateOutputDto>> Handle(MeterReadingExcelFileDownloadDto input, CancellationToken cancellationToken);
    }
}