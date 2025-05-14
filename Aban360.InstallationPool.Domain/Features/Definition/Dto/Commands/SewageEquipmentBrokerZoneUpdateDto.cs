namespace Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

public record SewageEquipmentBrokerZoneUpdateDto
{
    public short Id { get; set; }
    public int ZoneId { get; set; }
    public short SewageEquipmentBrokerId { get; set; }
}
