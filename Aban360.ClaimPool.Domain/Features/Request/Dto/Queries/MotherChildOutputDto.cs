namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record MotherChildOutputDto
    {
        public MotherOutputDto MotherInfo { get; set; }
        public MotherOutputDto InheritedInfo { get; set; }
        public MotherChildOutputDto(MotherOutputDto motherInfo, MotherOutputDto inheritedInfo)
        {
            MotherInfo = motherInfo;
            InheritedInfo = inheritedInfo;
        }
    }
}
