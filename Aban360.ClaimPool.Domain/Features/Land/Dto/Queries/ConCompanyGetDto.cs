namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ConCompanyGetDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string CompanyName { get; set; }
        public string RepresentativeName { get; set; }
        public string? CompanyNationalCode { get; set; }
        public string? RepresentativeNationalCode { get; set; }
        public string? RepresentativeFatherName { get; set; }
        public string CompanyMobileNumber { get; set; }
        public string RepresentativeMobileNumber { get; set; }
        public string? CompanyAddress { get; set; }
        public string? RepresentativeAddress { get; set; }
        public string? CompanyPostalCode { get; set; }
        public string? RepresentativePostalCode { get; set; }
        public string? RepresentativeBirthDateJalali { get; set; }
        public string? RepresentativeBirthPlace { get; set; }
        public string? RepresentativeCertificateNumber { get; set; }
        public string? AdministratorName { get; set; }
        public string? AdministratorMobileNumber { get; set; }
        public string ContractNumber { get; set; }
        public string ContractSubject { get; set; }
        public string ContractDataJalali { get; set; }
        public int ContractDuration { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedDateTime { get; set; }
        public Guid? RemovedBy { get; set; }
        public DateTime? RemovedDateTime { get; set; }
    }
}
