namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UsageGroup1DuplicateInsertDto
    {
        public short Id { get; set; }
        public string Title { get; set; }
        public UsageGroup1DuplicateInsertDto(short id, string title)
        {
            Id = id;
            Title = title;
        }
        public UsageGroup1DuplicateInsertDto()
        {
        }
    }
}
