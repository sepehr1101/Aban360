namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsConfirmFileInputDto
    {
        public string FileId { get; set; }
        public CollectBillsConfirmFileInputDto(string fileId)
        {
            FileId = fileId;
        }
        public CollectBillsConfirmFileInputDto()
        {
        }
    }
}
