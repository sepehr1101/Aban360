
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.InstallationPool.Domain.Features.Definition.Entities;

[Table(nameof(EquipmentBroker))]
public class EquipmentBroker
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string Website { get; set; } = null!;

    public string ApiUrl { get; set; } = null!;

    public virtual ICollection<EquipmentBrokerZone> EquipmentBrokerZones { get; set; } = new List<EquipmentBrokerZone>();
}