namespace Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

public record EquipmentBrokerZoneUpdateDto
{
    public short Id { get; set; }
    public int ZoneId { get; set; }
    public short EquipmentBrokerId { get; set; }
}
