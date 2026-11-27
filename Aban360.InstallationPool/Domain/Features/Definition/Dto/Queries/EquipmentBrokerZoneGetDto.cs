namespace Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;

public record EquipmentBrokerZoneGetDto
{
    public short Id { get; set; }
    public int ZoneId { get; set; }
    public string ZoneTitle { get; set; } = null!;
    public short EquipmentBrokerId { get; set; }
}