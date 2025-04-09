using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.Contexts.Implementations;

public partial class BlobPoolContext
{
    public virtual DbSet<DocumentCategory> DocumentCategories { get; set; }
    public virtual DbSet<DocumentType> DocumentTypes{ get; set; }
    public virtual DbSet<ExecutableMimetype> ExecutableMimetypes{ get; set; }
    public virtual DbSet<MimetypeCategory> MimetypeCategories{ get; set; }
    public virtual DbSet<Document> Documents{ get; set; }

}
