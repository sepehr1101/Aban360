using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerUpdateDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
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
        public int ImprovementCommertial { get; set; }
        public int ImprovementDomestic { get; set; }
        public int ImprovementOverall { get; set; }
        public int Premises { get; set; }
        public int HouseholdNumber { get; set; }
        public string HouseholdDateJalali { get; set; }
        public int MeterDiamterId { get; set; }
        public bool IsSpecial { get; set; }
        public int ContractualCapacity { get; set; }
        public string ToDayDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string ToDayDateJalaliWithFragmentYear { get; set; } = DateTime.Now.ToShortPersianDateString().Substring(2, 8);


        public string MeterInstallationDateJalali { get; set; }
        public string MeterRequestDateJalali { get; set; }
        public string? SewageInstallationDateJalali { get; set; }
        public string? SewageRequestDateJalali { get; set; }
        public int Siphon100 { get; set; }
        public int Siphon125 { get; set; }
        public int Siphon150 { get; set; }
        public int Siphon200 { get; set; }
        public int Siphon5 { get; set; }
        public int Siphon6 { get; set; }
        public int Siphon7 { get; set; }
        public int Siphon8 { get; set; }
        public int MainSiphon { get; set; }
        public int CommonSiphon { get; set; }
        public int Operator { get; set; }

        public int DeletionStateId { get; set; }
        public string BodySerial { get; set; }
        public string MeterRegisterDateJalali { get; set; }
        public string? SewageRegisterDateJalali { get; set; }
        public int GuildId { get; set; }

    }
}
