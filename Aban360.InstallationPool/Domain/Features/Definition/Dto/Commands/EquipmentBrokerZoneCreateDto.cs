namespace Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

public record EquipmentBrokerZoneCreateDto
{
    public int ZoneId { get; set; } 
    public short EquipmentBrokerId { get; set; }
}