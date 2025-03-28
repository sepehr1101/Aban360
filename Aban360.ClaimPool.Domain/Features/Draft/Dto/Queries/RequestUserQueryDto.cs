using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries
{
    public record RequestUserQueryDto
    {
        public EstateCommandDto EstateCommand { get; set; }
        public IndividualCommandDto IndividualCommand { get; set; }
        public SiphonCommandDto SiphonCommand { get; set; }
        public WaterMeterCommandDto WaterMeterCommand { get; set; }
    }
}
