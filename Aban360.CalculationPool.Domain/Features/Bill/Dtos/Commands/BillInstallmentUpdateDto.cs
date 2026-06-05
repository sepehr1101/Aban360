namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record BillInstallmentUpdateDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int Id { get; set; }
        public string DeadLineDateJalali { get; set; }
        public long Amount { get; set; }
        public BillInstallmentUpdateDto(int zoneId,int customerNumber,int id,string deadlineDateJalali,long amount)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            Id = id;
            DeadLineDateJalali = deadlineDateJalali;
            Amount = amount;
        }
        public BillInstallmentUpdateDto()
        {
        }
    }
}
