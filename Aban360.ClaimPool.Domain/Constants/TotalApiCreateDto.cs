using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;

namespace Aban360.ClaimPool.Domain.Constants
{
    public record TotalApiCreateDto
    {
        public EstateCreateDto Estate { get; set; }
        public WaterMeterCreateDto WaterMeter { get; set; }
        public ICollection<SiphonCreateDto> siphons { get; set; }
        public ICollection<IndividualCreateDto> individuals { get; set; }
    }
}
