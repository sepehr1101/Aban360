namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record BillFindReturnedInputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public string PreviousDateJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public BillFindReturnedInputDto(int zoneId, int customerNumber, int previousNumber, int currentNumber, string previousDateJalali, string currentDateJalali)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;    
            CurrentNumber = currentNumber;
            PreviousNumber = previousNumber;
            PreviousDateJalali = previousDateJalali;
            CurrentDateJalali = currentDateJalali;
        }
        public BillFindReturnedInputDto()
        {
        }
    }
}
