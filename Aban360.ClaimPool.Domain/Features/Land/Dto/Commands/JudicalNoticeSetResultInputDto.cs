namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record JudicalNoticeSetResultInputDto
    {
        public int Id { get; set; }
        public int ResultId { get; set; }
        public string JudicialId { get; set; }
        public string? Description { get; set; }
    }
}
