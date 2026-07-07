namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkUnconfirmedDataOutputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string RegisterDateJalali { get; set; }
        public int Amount { get; set; }
        public int DescriptionCode{ get; set; }//DiscountTypeId 
        public string DescriptionTitle { get; set; }//DiscountTypeTitle
        public int DiscountAmount { get; set; }
        public int Type { get; set; }
        public string TypeTitle { get; set; }
        public int OfferingId { get; set; }
        public string OfferingTitle { get; set; }

    }
}
