namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CollectBillsDetailUpdateDto
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? Description { get; set; }
        public DateTime FinishDateTime { get; set; }
        public CollectBillsDetailUpdateDto(int id, string fileName, string? description, DateTime finishDateTime)
        {
            Id = id;
            FileName = fileName;
            Description = description;
            FinishDateTime = finishDateTime;
        }
        public CollectBillsDetailUpdateDto()
        {
        }
    }
}
