namespace Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

public record SewageEquipmentBrokerUpdateDto
{
    public short Id { get; set; }
    public string Title { get; set; } = null!;
    public string Website { get; set; } = null!;
    public string ApiUrl { get; set; } = null!;
}
