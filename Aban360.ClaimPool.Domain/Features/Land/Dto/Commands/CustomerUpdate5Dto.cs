namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record CustomerUpdate5Dto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }

        public int MeterDiameterId { get; set; }
        public string BodySerial { get; set; }
        public int CommonSiphon { get; set; }
        public int MainSiphon { get; set; }

        public int Siphon100 { get; set; }
        public int Siphon125 { get; set; }
        public int Siphon150 { get; set; }
        public int Siphon200 { get; set; }
        public int Siphon5 { get; set; }
        public int Siphon6 { get; set; }
        public int Siphon7 { get; set; }
        public int Siphon8 { get; set; }

        public string MeterInstallationDateJalali { get; set; }
        public string MeterRequestDateJalali { get; set; }
        public string MeterRegisterDateJalali { get; set; }
        public string? SewageInstallationDateJalali { get; set; }
        public string? SewageRequestDateJalali { get; set; }
        public string? SewageRegisterDateJalali { get; set; }

        public string HouseholdDateJalali { get; set; }
    }
}
