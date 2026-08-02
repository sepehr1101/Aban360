namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UsageGroup1DuplicateInsetInputDto
    {
        public short UsageGroup1Id { get; set; }
        public string UsageGroup1Title { get; set; }
        public bool IsConfirm { get; set; }
    }
}
