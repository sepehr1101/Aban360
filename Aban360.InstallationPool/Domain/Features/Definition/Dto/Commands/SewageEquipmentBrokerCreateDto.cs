namespace Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

public record SewageEquipmentBrokerCreateDto
{
    public string Title { get; set; } = null!;
    public string Website { get; set; } = null!;
    public string ApiUrl { get; set; } = null!;
}
