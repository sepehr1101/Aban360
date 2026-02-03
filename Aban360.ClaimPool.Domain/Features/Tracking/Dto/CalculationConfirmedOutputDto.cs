using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Domain.Features.Tracking.Dto
{
    public record CalculationConfirmedOutputDto
    {
        public string ZoneTitle { get; set; }
        public int ZoneId { get; set; }
        public string RegionTitle { get; set; }
        public int RegionId { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int Siphon100 { get; set; }
        public int Siphon125 { get; set; }
        public int Siphon150 { get; set; }
        public int Siphon200 { get; set; }
        public int Premises { get; set; }
        public int ImprovementOverall { get; set; }
        public int ImprovementDomestic { get; set; }
        public int ImprovementCommertial { get; set; }
        public int CommertialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int FamilyCount { get; set; }
        public int HouseholdNumber { get; set; }
        public int DiscountTypeId { get; set; }
        public string DiscountTypeTitle { get; set; }
        public bool HasBroker { get; set; }
        public int ContractualCapacity { get; set; }
        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; }
        public int TrackNumber { get; set; }
        public string BillId { get; set; }
        public string NeighbourBillId { get; set; }
        public int RegionMultiplier { get; set; }
        public int MeterTypeId { get; set; }
        public string MeterTypeTitle { get; set; }
        public int MeterDiamterId { get; set; }
        public string MeterDiamterTitle { get; set; }
        public int DiscountCount { get; set; }
        public string PostalCode { get; set; }

        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string MessageNumber { get; set; }
        public string Address { get; set; }
        public string? Description { get; set; }
        public ICollection<NumericDictionary> CompanyServiceSelected { get; set; }
    }
}