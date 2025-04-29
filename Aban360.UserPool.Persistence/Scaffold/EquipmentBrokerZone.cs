using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class EquipmentBrokerZone
{
    public short Id { get; set; }

    public int ZoneId { get; set; }

    public string ZoneTitle { get; set; } = null!;

    public short EquipmentBrokerId { get; set; }

    public virtual EquipmentBroker EquipmentBroker { get; set; } = null!;
}
