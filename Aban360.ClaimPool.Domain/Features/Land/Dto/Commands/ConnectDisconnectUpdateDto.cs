namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record ConnectDisconnectUpdateDto
    {
        public long Id { get; set; }
        public DateTime ResultDateTime { get; set; } = DateTime.Now;
        public Guid ResultBy { get; set; }
        public int ResultId { get; set; }
        public string ResultTitle { get; set; }
        public string? JudicialNoticeId { get; set; }
        public string? Description { get; set; }
        public ConnectDisconnectUpdateDto(long id, Guid resultBy, int resultId, string resultTitle, string? judicialNoticeId, string? description)
        {
            Id = id;
            ResultBy = resultBy;
            ResultId = resultId;
            ResultTitle = resultTitle;
            JudicialNoticeId = judicialNoticeId;
            Description = description;
        }
        public ConnectDisconnectUpdateDto()
        {
        }
    }
}
