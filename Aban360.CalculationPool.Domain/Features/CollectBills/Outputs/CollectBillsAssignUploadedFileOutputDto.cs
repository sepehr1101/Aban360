namespace Aban360.CalculationPool.Domain.Features.CollectBills.Outputs
{
    public record CollectBillsAssignUploadedFileOutputDto
    {
        public string Description { get; set; }
        public long TraceNumber { get; set; }
        public string CompressedFileName { get; set; }
        public string CompressedFileExtension { get; set; }
    }
}
