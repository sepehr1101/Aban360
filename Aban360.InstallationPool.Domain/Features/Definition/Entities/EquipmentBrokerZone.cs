using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.InstallationPool.Domain.Features.Definition.Entities;

[Table(nameof(EquipmentBrokerZone))]
public class EquipmentBrokerZone
{
    public short Id { get; set; }

    public int ZoneId { get; set; } 

    public string ZoneTitle { get; set; } = null!;

    public short EquipmentBrokerId { get; set; }

    public virtual EquipmentBroker EquipmentBroker { get; set; } = null!;
}