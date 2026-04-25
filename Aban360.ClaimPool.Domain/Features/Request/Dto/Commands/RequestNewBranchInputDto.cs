namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record RequestNewBranchInputDto
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string NationalCode { get; set; }
        public string CertificataNumber { get; set; }
        public string Address { get; set; }
        public string NeighbourBillId { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }
        public bool HasSendSms { get; set; }
        public ICollection<int> SelectedServices { get; set; }

    }
}
