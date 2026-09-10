namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record BillReadingListDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string ReadingNumber { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public string RegisterDayJalali { get; set; }
        public string CurrentDateJalali { get; set; }
        public int CurrentNumber { get; set; }
        public int CounterStateCode { get; set; }
        public string CounterStateTitle {get;set;}
        public int DeletionStateId{get;set;}
        public string DeletionStateTitle {get;set;}


    }
}
