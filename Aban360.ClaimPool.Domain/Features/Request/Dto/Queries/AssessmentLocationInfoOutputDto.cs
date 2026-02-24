namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record AssessmentLocationInfoOutputDto
    {
        public Guid TrackId { get; set; }
        public string MobileNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string NotificationMobileNumber { get; set; }
        public string? BillId { get; set; }
        public string? NeighbourBillId { get; set; }
        public string StringTrackNumber { get; set; }
        public int TrackNumber { get; set; }
        public int CustomerNumber { get; set; }
        public int ServiceGroupId { get; set; }
        public string ServiceGroupTitle { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int DiscountCount { get; set; }
        public string NationalCode { get; set; }
        public string FatherName { get; set; }
        public int HouseValue { get; set; }
        public string? Description { get; set; }
        public bool IsNonPermanent { get; set; }
        public bool HasCustomerNumber { get; set; }
        public string FullName { get; set; }
        public bool IsVisited { get; set; }
        public string DiscountTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int MeterDiameterId { get; set; }
        public string MeterDiameterTitle { get; set; }
        public int? AssessmentCode { get; set; }
        public string? AssessmentDateJalali { get; set; }
        public string? AssessmentName { get; set; }
        public string? AssessmentMobileNumber { get; set; }

        public string CertificateNumber { get; set; }//
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string ReadingNumber { get; set; }
        public string BranchTypeTitle { get; set; }
        public int BranchTypeId { get; set; }
        public int ContractualCapacity { get; set; }
        public int Premises { get; set; }
        public int ImprovementOverall { get; set; }
        public int ImprovementDomestic { get; set; }
        public int ImprovementCommercial { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public string? LicenseIssuanceDateJalali { get; set; }//
        public string? BlockCode { get; set; }
        public int Siphon100 { get; set; }
        public int Siphon125 { get; set; }
        public int Siphon150 { get; set; }
        public int Siphon200 { get; set; }
        public int MainSiphon { get; set; }
        public int? TrenchLenW { get; set; }
        public int? TrenchLenS { get; set; }
        public int? AsphaltLenW { get; set; }
        public int? AsphaltLenS { get; set; }
        public int? RockyLenW { get; set; }
        public int? RockyLenS { get; set; }
        public int? OtherLenW { get; set; }
        public int? OtherLenS { get; set; }
        public int? BasementDepth { get; set; }
        public IEnumerable<ServiceGroupWithCheckedOutputDto> ServiceGroups { get; set; }
    }
}