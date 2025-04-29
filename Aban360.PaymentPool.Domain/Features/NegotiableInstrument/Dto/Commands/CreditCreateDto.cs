using Aban360.PaymentPool.Domain.Constansts;

namespace Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands
{
    public record CreditCreateDto
    {
        public string BillId { get; set; } = null!;
        public string PaymentId { get; set; } = null!;
        public long InvoiceId { get; set; }
        public long InvoiceInstallmentId { get; set; }
        public long Amount { get; set; }
        public short UploaderId { get; set; }
        public CreditorTypeEnum CreditorTypeId { get; set; }
    }
}
