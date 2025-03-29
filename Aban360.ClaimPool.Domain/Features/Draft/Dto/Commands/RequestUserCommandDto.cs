using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record RequestUserCommandDto
    {
        public EstateCommandDto EstateCommand { get; set; }
        public FlatCommandDto flatCommands { get; set; }
        public IndividualCommandDto IndividualCommand { get; set; }
        public SiphonCommandDto SiphonCommand { get; set; }
        public WaterMeterCommandDto WaterMeterCommand { get; set; }
        public WaterMeterSiphonCommandDto WaterMeterSiphonCommand { get; set; }
        public WaterMeterTagCommandDto WaterMeterTagCommand { get; set; }
        public IndividualEstateCommandDto IndividualEstateCommand { get; set; }
        public IndividualTagCommandDto IndividualTagCommand { get; set; }
    }
}
