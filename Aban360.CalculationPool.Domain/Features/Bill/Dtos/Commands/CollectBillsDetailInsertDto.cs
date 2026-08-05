namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CollectBillsDetailInsertDto
    {
        public Guid GroupingId { get; set; }
        public int StepId { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string? Description { get; set; }
        public CollectBillsDetailInsertDto(Guid groupingId, int stepId, DateTime insertDateTime, string? description)
        {
            GroupingId = groupingId;
            StepId = stepId;
            InsertDateTime = insertDateTime;
            Description = description;
        }
        public CollectBillsDetailInsertDto()
        {
        }
    }
}
