namespace Aban360.CalculationPool.Domain.Features.CollectBills.Outputs
{
    public record CollectBillsUploadOutputDto
    {
        public string FileID { get; set; }
        public CollectBillsUploadOutputDto(string fileId)
        {
            FileID = fileId;
        }
        public CollectBillsUploadOutputDto()
        {
        }
    }
}