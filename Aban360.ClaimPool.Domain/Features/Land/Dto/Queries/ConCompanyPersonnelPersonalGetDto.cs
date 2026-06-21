namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ConCompanyPersonnelPersonalGetDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string? NationalCode { get; set; }
    }
}
