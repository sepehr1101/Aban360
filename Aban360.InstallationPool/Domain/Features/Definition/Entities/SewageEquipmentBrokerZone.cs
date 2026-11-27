using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.InstallationPool.Domain.Features.Definition.Entities;

[Table(nameof(SewageEquipmentBrokerZone))]
public class SewageEquipmentBrokerZone
{
    public short Id { get; set; }

    public int ZoneId { get; set; } 

    public string ZoneTitle { get; set; } = null!;

    public short SewageEquipmentBrokerId { get; set; }

    public virtual SewageEquipmentBroker SewageEquipmentBroker { get; set; } = null!;
}