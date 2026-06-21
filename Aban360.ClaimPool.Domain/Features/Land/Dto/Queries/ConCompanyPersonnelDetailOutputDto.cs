namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ConCompanyPersonnelDetailOutputDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public string PersonnelCode { get; set; }
        public string? HomeAddress { get; set; }
        public string? HomePhoneNumber { get; set; }
        public string? EducationGrade { get; set; }
        public string? EducationField { get; set; }
        public string? BirtDateJalali { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedDateTime { get; set; }
        public Guid? RemovedBy { get; set; }
        public DateTime? RemovedDateTime { get; set; }
    }
}
