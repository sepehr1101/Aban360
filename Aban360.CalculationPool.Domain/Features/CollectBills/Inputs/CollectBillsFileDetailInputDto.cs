namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsFileDetailInputDto
    {
        public string FileId { get; set; }
        public CollectBillsFileDetailInputDto(string fileId)
        {
            FileId = fileId;
        }
        public CollectBillsFileDetailInputDto()
        {
        }
    }
}
