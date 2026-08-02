namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record UsageGroup1DuplicateInsertOutputDto
    {
        public int UsageGroup1Count { get; set; }
        public int UsageGroup2Count { get; set; }
        public int UsageGroup3Count { get; set; }

        public short UsageGroup1Id { get; set; }
        public bool IsConfirm { get; set; }
        public UsageGroup1DuplicateInsertOutputDto(int UsageGroup1, int UsageGroup2, int UsageGroup3, short usageGroup1Id, bool isConfirm)
        {
            UsageGroup1Count = UsageGroup1;
            UsageGroup2Count = UsageGroup2;
            UsageGroup3Count = UsageGroup3;
            UsageGroup1Id = usageGroup1Id;
            IsConfirm = isConfirm;
        }
        public UsageGroup1DuplicateInsertOutputDto()
        {
        }
    }
}
