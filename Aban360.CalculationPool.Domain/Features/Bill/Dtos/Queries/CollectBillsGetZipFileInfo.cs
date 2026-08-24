namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record CollectBillsGetZipFileInfo
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public CollectBillsGetZipFileInfo(string filePath, string fileName)
        {
            FilePath = filePath;
            FileName = fileName;
        }
        public CollectBillsGetZipFileInfo()
        {
        }
    }
}
