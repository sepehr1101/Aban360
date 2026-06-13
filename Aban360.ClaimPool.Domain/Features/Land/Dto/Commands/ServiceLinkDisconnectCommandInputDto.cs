namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record ServiceLinkDisconnectCommandInputDto
    {
        public string BillId { get; set; }
        public string Who { get; set; }
        public string? Description { get; set; }
        public string Why { get; set; } 
    }
}
