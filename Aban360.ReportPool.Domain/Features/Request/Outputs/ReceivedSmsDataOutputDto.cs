namespace Aban360.ReportPool.Domain.Features.Request.Outputs
{
    public record ReceivedSmsDataOutputDto
    {
        public Guid Id { get; set; }
        public string SenderNumber { get; set; }
        public string RecipientNumber { get; set; }
        public string Text { get; set; }
        public string DateJalali { get; set; }
        public string Time { get; set; }
        public DateTime DateAndTime { get; set; }
    }
}
