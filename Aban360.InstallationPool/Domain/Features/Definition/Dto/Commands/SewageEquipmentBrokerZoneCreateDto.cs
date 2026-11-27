namespace Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

public record SewageEquipmentBrokerZoneCreateDto
{
    public int ZoneId { get; set; } 
    public short SewageEquipmentBrokerId { get; set; }
}
