using System;
using System.Collections.Generic;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class EquipmentBroker
{
    public short Id { get; set; }

    public string Title { get; set; } = null!;

    public string Website { get; set; } = null!;

    public string ApiUrl { get; set; } = null!;

    public virtual ICollection<EquipmentBrokerZone> EquipmentBrokerZones { get; set; } = new List<EquipmentBrokerZone>();
}
