namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record InvoiceCreateDto
    {
        public short InvoiceTypeId { get; set; }
        public short InvoiceStatusId { get; set; }
        public short DepositRate { get; set; }
        public short InstallmentCount { get; set; }
    }
}
