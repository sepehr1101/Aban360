namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CollectBillsDetailUpdateDto
    {
        public int Id { get; set; }
        public DateTime FinishDateTime { get; set; }
        public CollectBillsDetailUpdateDto(int id, DateTime finishDateTime)
        {
            Id = id;
            FinishDateTime = finishDateTime;
        }
        public CollectBillsDetailUpdateDto()
        {
        }
    }
}
