namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record MeterReadingCartableDataOutputDto
    {
        public Guid Id{get;set;}
        public Guid FirstTrackingId {get;set;}
        public Guid BlockId {get;set;}
        public int TrackNumber{get;set;}
        public int ZoneId{get;set;}
        public string ZoneTitle {get;set;}
        public int FromReadingNumber{get;set;}
        public int ToReadingNumber{get;set;}
        public string TrackingStartDateJalali {get;set;}
        public DateTime TrackingStartDateTime{get;set;}
        public string CounterReaderCode {get;set;}
        public string CounterReaderName{get;set;}
        public Guid CounterReaderId {get;set;}
        public int ItemCustomerCount{get;set;}
        public int OverallCustomerCount { get; set; }

    }
}
