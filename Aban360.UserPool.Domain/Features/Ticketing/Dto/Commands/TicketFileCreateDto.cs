namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands
{
    public record TicketFileCreateDto
    {
        public string OriginalFileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[] FileContent { get; set; } = null!;
    }
}
