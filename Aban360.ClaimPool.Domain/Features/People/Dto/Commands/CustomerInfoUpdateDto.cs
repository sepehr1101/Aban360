namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record CustomerInfoUpdateDto
    {
        public string BillId { get; set; }
        public int ZoneId { get; set; }
        public string ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public int MeterDiameterId { get; set; }
        public int UsageId { get; set; }
        public int OtherUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int HouseholdNumber { get; set; }
        public int DomesticUnit { get; set; }
        public string RegisterDateJalali { get; set; }
        public int Premises { get; set; }
        public int OverollImprovement { get; set; }
        public int CommercialImprovement { get; set; }
        public int DomesticImprovement { get; set; }
        public string MeterRequestDateJalali { get; set; }
        public string MeterInstallationDateJalali { get; set; }
        public string SiphonRequestDateJalali { get; set; }
        public string SiphonInstallationDateJalali { get; set; }
        public string Address { get; set; }
        public string HousePlate { get; set; }
        public bool IsSpecial { get; set; }
        public int DeletionStateId { get; set; }
        public int UseStateId { get; set; }
        public int MainSiphon { get; set; }
        public int Siphon1 { get; set; }
        public int Siphon2 { get; set; }
        public int Siphon3 { get; set; }
        public int Siphon4 { get; set; }
        public int CommonSiphon1 { get; set; }
        public int ContractualCapacity { get; set; }
        public string BodySerial { get; set; }
        public string WaterInstalltionRegistareDate { get; set; }
        public string SewageInstalltionRegistareDate { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string NationalCode { get; set; }
        public int Siphon5 { get; set; }
        public int Siphon6 { get; set; }
        public int Siphon7 { get; set; }
        public int Siphon8 { get; set; }
        public int MOJAVZ { get; set; }//
        public string VillageId { get; set; }
        public string VillageName { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public int EmptyUnit { get; set; }
        public int Operator { get; set; }
        public int Guild { get; set; }
        public string date_KHANE { get; set; }
    }
}
