namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record IndividualsInfoDto
    {
        public string FullName { get; set; } = null!;
        public string? NationalId { get; set; }
        public string? FatherName { get; set; }
        public string? PhoneNumbers { get; set; }
        public string? MobileNumbers { get; set; }
        public string? IndividualEstateRelationType { get; set; }
        public short  HouseholderNumber { get; set; }
        public short NumberOfPeople { get; set; }//
        public string DiscountType { get; set; }
        public bool IsOwnerAgent { get; set; }//
    }
}
