namespace Aban360.ReportPool.Domain.Features.Tagging
{
    public record BillIdTagRemoveByTagIdsOutputDto
    {
        public int BillCount { get; set; }
        public int RecordCount { get; set; }
        public bool IsRemoved { get; set; }
        public BillIdTagRemoveByTagIdsOutputDto(int billCount, int recordCount, bool isRemoved)
        {
            BillCount = billCount;
            RecordCount = recordCount;
            IsRemoved = isRemoved;
        }
        public BillIdTagRemoveByTagIdsOutputDto()
        {
        }
    }
}
