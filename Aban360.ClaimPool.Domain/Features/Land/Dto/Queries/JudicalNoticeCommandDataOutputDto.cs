namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record JudicalNoticeCommandDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string RegionTitle { get; set; }

        //Customer
        public string CustomerBillId { get; set; }
        public int CustomerNumber { get; set; }
        public string CustomerReadingNumber { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerFatherName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPostalCode { get; set; }
        public string CustomerMobileNumber { get; set; }
        public string CustomerNationalCode { get; set; }
        public long DebtAmount { get; set; }
        public string CustomerCertificateNumber { get; set; }//
        public string CustomerBirthPlace { get; set; }//
        public string? CustomerBirthDateJalali { get; set; }//

        //Representative
        public string CompanyName { get; set; }
        public string? CompanyNationalCode { get; set; }
        public string CompanyMobileNumber { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyPostalCode { get; set; }

        public string RepresentativeName { get; set; }
        public string? RepresentativeNationalCode { get; set; }
        public string? RepresentativeFatherName { get; set; }
        public string RepresentativeMobileNumber { get; set; }
        public string? RepresentativeAddress { get; set; }
        public string? RepresentativePostalCode { get; set; }
        public string? RepresentativeBirthDateJalali { get; set; }
        public string? RepresentativeBirthPlace { get; set; }
        public string? RepresentativeCertificateNumber { get; set; }
        public string? AdministratorName { get; set; }
        public string? AdministratorMobileNumber { get; set; }
        public string ContractNumber { get; set; }
        public string ContractDataJalali { get; set; }

    }
}
