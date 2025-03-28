namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record RequestUserCommandDto
    {
        public EstateCommandDto EstateCommand { get; set; }
        public IndividualCommandDto IndividualCommand { get; set; }
        public SiphonCommandDto SiphonCommand { get; set; }
        public WaterMeterCommandDto WaterMeterCommand { get; set; }
    }
}
