namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands
{
    public record BankFileStructureCreateDto
    {
        public short FromIndex { get; set; }
        public short ToIndex { get; set; }
        public short StringLenght { get; set; }
        public string Title { get; set; } = null!;
        public bool IsHeader { get; set; } = false;
        public short BankId { get; set; }

    }
}
