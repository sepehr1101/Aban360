namespace Aban360.Common.BaseEntities
{
    public record ZoneIdAndCustomerNumber
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public int DeletionStateId { get; set; }
    }
}
