using Microsoft.AspNetCore.Http;

namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public record BillIdTagInsertByExcelFileInputDto
    {
        public IFormFile ExcelFile { get; set; }
    }
}
