namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record MotherChildOutputDto
    {
        public bool HasMother { get; set; }
        public MotherOutputDto? MotherInfo { get; set; }
        public MotherOutputDto? InheritedInfo { get; set; }
        public MotherChildOutputDto(MotherOutputDto? motherInfo, MotherOutputDto? inheritedInfo,bool hasMother)
        {
            MotherInfo = motherInfo;
            InheritedInfo = inheritedInfo;
            HasMother = hasMother;
        }
    }
}
