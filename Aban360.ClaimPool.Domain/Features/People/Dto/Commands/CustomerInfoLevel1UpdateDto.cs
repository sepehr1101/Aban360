namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record CustomerInfoLevel1UpdateDto
    {
        public string BillId { get; set; }
        public int ZoneId { get; set; }
        public string FirstName { get; set; }
        public string  Surname { get; set; }
        public string FatherName { get; set; }
        public string MobileNumber { get; set; }
        public string NationalCode { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
    }
}