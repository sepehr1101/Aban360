namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsConfirmFileInputDto
    {
        public long FileId { get; set; }
        public CollectBillsConfirmFileInputDto(long fileId)
        {
            FileId = fileId;
        }
        public CollectBillsConfirmFileInputDto()
        {
        }
    }
}
