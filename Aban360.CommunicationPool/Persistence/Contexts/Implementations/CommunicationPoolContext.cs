using Aban360.Common.Db.Context;
using Aban360.CommunicationPool.Persistence.Constants;
using Aban360.CommunicationPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CommunicationPool.Persistence.Contexts.Implementations
{
    public partial class CommunicationPoolContext: BaseDbContext,IUnitOfWork
    {
        public CommunicationPoolContext()
        {}

        public CommunicationPoolContext(DbContextOptions<CommunicationPoolContext> options)
            :base(options)
        {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(TableSchema.Name);
            OnModelCreatingPartial(modelBuilder);   
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
