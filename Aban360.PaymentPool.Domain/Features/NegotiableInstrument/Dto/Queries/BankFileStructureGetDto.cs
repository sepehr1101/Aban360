namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries
{
    public record BankFileStructureGetDto
    {
        public short Id { get; set; }
        public short FromIndex { get; set; }
        public short ToIndex { get; set; }
        public short StringLenght { get; set; }
        public string Title { get; set; } = null!;
        public bool IsHeader { get; set; } = false;
    }
}
