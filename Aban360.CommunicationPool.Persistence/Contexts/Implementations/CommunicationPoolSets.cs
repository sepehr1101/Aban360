using Aban360.CommunicationPool.Domain.Features.Hubs.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CommunicationPool.Persistence.Contexts.Implementations
{
    public partial class CommunicationPoolContext
    {
        public virtual DbSet<HubEvent> HubEvents { get; set; }
    }
}
