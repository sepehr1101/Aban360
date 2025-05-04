namespace Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries
{
    public record BankGetDto
    {
        public short Id { get; set; }
        public string BankName { get; set; } = null!;
        public string? _3Char { get; set; }
        public string? Icon { get; set; }
        public string CentralBankCode { get; set; } = null!;
        public string? Description { get; set; }
    }
}
