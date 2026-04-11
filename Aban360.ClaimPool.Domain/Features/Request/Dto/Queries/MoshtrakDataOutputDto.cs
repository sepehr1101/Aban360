using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.BaseEntities;
using Scrutor;

namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record MoshtrakDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string? ReadingNumber { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string RequestDateJalali { get; set; }
        public string Address { get; set; }
        public string? PostalCode { get; set; }
        public string? NeighbourBillId { get; set; }
        public int TrackNumber { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public bool IsRegistered { get; set; }

        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; }
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
        public string MeterDiameterTitle { get; set; }
        public int MeterDiameterId { get; set; }
        public int DiscountTypeId { get; set; }
        public string DiscountTypeTitle { get; set; }
        public int DiscountCount { get; set; }
        public bool IsSpecial { get; set; }
        public bool CounterType { get; set; }
        public string? NotificationMobile { get; set; }
        public string? Description { get; set; }
        public int HouseValue { get; set; }
        public bool IsNonPermanent { get; set; }
        public string? BlockId { get; set; }
        public int BrokerId { get; set; }//has=1,2 hasnt=0
        public bool HasBroker { get { return BrokerId >= 1; } }
        public IEnumerable<MoshtrakCompanyService> CompanyServiceItems { get; set; }
    }
}
