namespace Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands
{
    public record NotifyTextMessageOutput
    {
        public string Color { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int Seconds { get; set; }
    }
}
