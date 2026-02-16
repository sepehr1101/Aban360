namespace Aban360.ReportPool.Domain.Features.Usp.Output
{
    public record UspPayment2Output
    {
        public int cod_enshab { get; set; }
        public long pard { get; set; }
        public long pard1 { get; set; }
        public long ted1 { get; set; }
        public long pard2 { get; set; }
        public long ted2 { get; set; }
        public long pard3 { get; set; }
        public long ted3 { get; set; }
        public long pard4 { get; set; }
        public long ted4 { get; set; }
        public long tedad { get; set; }
        public string? Karbari { get; set; }
    }
    public record UspPayment2Header
    {
        public long pard { get; set; }
        public long pard1 { get; set; }
        public long ted1 { get; set; }
        public long pard2 { get; set; }
        public long ted2 { get; set; }
        public long pard3 { get; set; }
        public long ted3 { get; set; }
        public long pard4 { get; set; }
        public long ted4 { get; set; }
        public long tedad { get; set; }
    }
}
