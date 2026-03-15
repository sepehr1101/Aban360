namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record MoshtrakUpdateInputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string? ReadingNumber { get; set; }
        public int TrackNumber { get; set; }

        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int UsageId { get; set; }
        public bool IsRegistered { get; set; }

        public int BranchTypeId { get; set; }
        public int Premises { get; set; }
        public int ImprovementOverall { get; set; }
        public int ImprovementDomestic { get; set; }
        public int ImprovementCommercial { get; set; }
        public int OtherUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int ContractualCapacity { get; set; }
        public int Siphon100 { get; set; }
        public int Siphon125 { get; set; }
        public int Siphon150 { get; set; }
        public int Siphon200 { get; set; }
        public int MainSiphon { get; set; }
        public int CommonSiphon { get; set; }
        public int MeterDiameterId { get; set; }
        public int DiscountTypeId { get; set; }
        public int DiscountCount { get; set; }
        public bool IsSpecial { get; set; }
        public bool CounterType { get; set; }//
        public string? NotificationMobile { get; set; }
        public string? Description { get; set; }
        public bool IsNonPermanent { get; set; }
        public int HouseValue { get; set; }

        public ICollection<int> SelectedServices { get; set; }
    }
}
