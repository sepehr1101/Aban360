namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record SubscriptionGetDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

        public string Plaque { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FatherName { get; set; }
        public int BranchTypeId { get; set; }
        public int UsageSellId { get; set; }
        public int UsageConsumptionId { get; set; }
        public int EmptyUnit { get; set; }
        public int CommertialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int HouseholdNumber { get; set; }
        public string HouseholdDateJalali { get; set; }
        public int MeterDiamterId { get; set; }
        public bool IsSpecial { get; set; }
        public int ContractualCapacity { get; set; }
    }
}
