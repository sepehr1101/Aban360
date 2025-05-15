using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.InstallationPool.Domain.Features.Definition.Entities;

[Table(nameof(SewageEquipmentBroker))]
public class SewageEquipmentBroker
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string Website { get; set; } = null!;

    public string ApiUrl { get; set; } = null!;

    public virtual ICollection<SewageEquipmentBrokerZone> EquipmentBrokerZones { get; set; } = new List<SewageEquipmentBrokerZone>();
}