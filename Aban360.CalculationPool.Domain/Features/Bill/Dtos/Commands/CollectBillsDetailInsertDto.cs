namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record CollectBillsDetailInsertDto
    {
        public Guid GroupingId { get; set; }
        public int StepId { get; set; }
        public DateTime InsertDateTime { get; set; }
        public DateTime? FinishDateTime { get; set; }
        public string? Description { get; set; }
        public CollectBillsDetailInsertDto(Guid groupingId, int stepId, DateTime insertDateTime, DateTime? finishDateTime, string? description)
        {
            GroupingId = groupingId;
            StepId = stepId;
            InsertDateTime = insertDateTime;
            FinishDateTime = finishDateTime;
            Description = description;
        }
        public CollectBillsDetailInsertDto()
        {
        }
    }
}
