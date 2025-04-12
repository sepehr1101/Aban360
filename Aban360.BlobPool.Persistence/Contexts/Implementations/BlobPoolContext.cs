using Aban360.BlobPool.Persistence.Constants;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Contexts.Implementations;

public partial class BlobPoolContext: BaseDbContext,IUnitOfWork
{
    public BlobPoolContext()
    {
    }

    public BlobPoolContext(DbContextOptions<BlobPoolContext> options)
        :base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuidler)
    {
        modelBuidler.HasDefaultSchema(TableSchema.Name);
        OnModelCreatingPartial(modelBuidler);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
