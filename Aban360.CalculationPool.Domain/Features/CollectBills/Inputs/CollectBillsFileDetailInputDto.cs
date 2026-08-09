namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsFileDetailInputDto
    {
        public long FileId { get; set; }
        public CollectBillsFileDetailInputDto(long fileId)
        {
            FileId = fileId;
        }
        public CollectBillsFileDetailInputDto()
        {
        }
    }
}
