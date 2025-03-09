using Aban360.MeterPool.Domain.Features.Management.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.MeterPool.Persistence.Contexts.Implementations
{
    public partial class MeterPoolContext
    {
        public virtual DbSet<ReadingPeriod> ReadingPeriods { get; set; }
        public virtual DbSet<ReadingPeriodType> ReadingPeriodTypes { get; set; }
        public virtual DbSet<ReadingConfigDefault> ReadingConfigDefaults { get; set; }
        public virtual DbSet<CounterState> CounterStates{ get; set; }
    }
}

