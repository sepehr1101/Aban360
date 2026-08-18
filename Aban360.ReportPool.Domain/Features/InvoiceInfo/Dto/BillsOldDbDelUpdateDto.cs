namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record BillsOldDbDelUpdateDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int TypeCode { get; set; }
        public BillsOldDbDelUpdateDto(int id, int zoneId, int customerNumber, int typCode)
        {
            Id = id;
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            TypeCode = typCode;
        }
        public BillsOldDbDelUpdateDto()
        {
        }
    }
}