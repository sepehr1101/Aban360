namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record OtherExpensesDataOutputDto
    {
        public int OfferingId { get; set; }
        public string OfferingTitle { get; set; }
        public long OfferingAmount { get; set; }
        public OtherExpensesDataOutputDto(int offeringId, string offeringTitle, long offeringAmount)
        {
            OfferingId = offeringId;
            OfferingTitle = offeringTitle;
            OfferingAmount = offeringAmount;
        }
    }
}
