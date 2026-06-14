namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record ServiceLinkConnectionInput
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public int? Why { get; set; }
        public string When { get; set; }//dateJalali
        public string Who { get; set; } = default!;
    }
}
