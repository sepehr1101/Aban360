using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Contexts.Implementation
{
    public partial class LocationPoolContext
    {
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CordinalDirection> CordinalDirections { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Headquarters> Headquarters { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Zone> Zones { get; set; }
        public virtual DbSet<Municipality> Municipalities { get; set; }
        public virtual DbSet<ReadingBound> ReadingBounds { get; set; }
        public virtual DbSet<ReadingBlock> ReadingBlocks { get; set; }
    }
}
