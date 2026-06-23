namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record JudicalNoticeCommandInputDto
    {
        public string BillId { get; set; }
        public int CompanyId { get; set; }
        public string? Description { get; set; }
    }
}
