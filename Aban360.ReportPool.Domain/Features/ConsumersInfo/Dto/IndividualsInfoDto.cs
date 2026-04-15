namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record IndividualsInfoDto
    {
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? NationalId { get; set; }
        public string? FatherName { get; set; }
        public string? PhoneNumbers { get; set; }
        public string? MobileNumbers { get; set; }
        public string? IndividualEstateRelationType { get; set; }
        public short HouseholdNumber { get; set; }
        public short NumberOfPeople { get; set; }//
        public string DiscountType { get; set; }
        public int DiscountTypeId { get; set; }
        public string? BlockCode { get; set; }
        public bool IsOwnerAgent { get; set; }//
    }
}
