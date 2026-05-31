namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record BillInstallmentUpdateDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int Id { get; set; }
        public string DueDateJalali { get; set; }
        public long Amount { get; set; }
        public BillInstallmentUpdateDto(int zoneId,int customerNumber,int id,string dueDateJalali,long amount)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            Id = id;
            DueDateJalali = dueDateJalali;
            Amount = amount;
        }
        public BillInstallmentUpdateDto()
        {
        }
    }
}
