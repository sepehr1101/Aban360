namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record ServiceLinkConnectionInput
    {
        public string BillId { get; set; } = default!;
        public string? Description { get; set; }
        public string Who { get; set; } = default!;
        public int? Why { get; set; }
    }
}
