using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Contexts.Implementation
{
    public partial class UserPoolContext
    {
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;
        public virtual DbSet<TicketMessage> TicketMessages { get; set; } = null!;
        public virtual DbSet<TicketAttachment> TicketAttachments { get; set; } = null!;
        public virtual DbSet<TicketStatus> TicketStatuses { get; set; } = null!;
        public virtual DbSet<TicketRating> TicketRatings { get; set; } = null!;

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(x => x.TicketNumber).HasMaxLength(50);
                entity.Property(x => x.Subject).HasMaxLength(255);
                entity.Property(x => x.StatusId).HasDefaultValue((short)1);
                entity.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                entity.Property(x => x.UpdatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                entity.HasIndex(x => x.TicketNumber).IsUnique();
                entity.HasIndex(x => new { x.StatusId, x.CreatedAt });
                entity.HasIndex(x => x.CreatedAt);
                entity.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.AssignedToUser).WithMany().HasForeignKey(x => x.AssignedToUserId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.Status).WithMany(x => x.Tickets).HasForeignKey(x => x.StatusId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<TicketMessage>(entity =>
            {
                entity.Property(x => x.Body).HasColumnType("nvarchar(max)");
                entity.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                entity.HasIndex(x => new { x.TicketId, x.CreatedAt, x.Id });
                entity.HasOne(x => x.Ticket).WithMany(x => x.Messages).HasForeignKey(x => x.TicketId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.SenderUser).WithMany().HasForeignKey(x => x.SenderUserId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<TicketAttachment>(entity =>
            {
                entity.ToTable("TicketAttachment", table => table.HasCheckConstraint("CK_TicketAttachment_SizeBytes", "[SizeBytes] >= 0"));
                entity.Property(x => x.FileGuid).HasDefaultValueSql("NEWID()");
                entity.HasIndex(x => x.FileGuid).IsUnique();
                entity.Property(x => x.OriginalFileName).HasMaxLength(255);
                entity.Property(x => x.ContentType).HasMaxLength(150);
                // FILESTREAM / ROWGUIDCOL are provisioned by FluentMigrator, not EF DDL.
                entity.Property(x => x.FileContent).HasColumnType("varbinary(max)");
                entity.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                entity.HasOne(x => x.TicketMessage).WithMany(x => x.Attachments).HasForeignKey(x => x.TicketMessageId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<TicketStatus>(entity =>
            {
                entity.Property(x => x.Id).ValueGeneratedNever();
                entity.Property(x => x.Code).HasMaxLength(50);
                entity.Property(x => x.Title).HasMaxLength(100);
                entity.HasIndex(x => x.Code).IsUnique();
            });

            modelBuilder.Entity<TicketRating>(entity =>
            {
                entity.ToTable("TicketRating", table => table.HasCheckConstraint("CK_TicketRating_Score", "[Score] BETWEEN 1 AND 5"));
                entity.Property(x => x.Comment).HasMaxLength(1023);
                entity.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                entity.HasOne(x => x.TicketMessage).WithOne(x => x.Rating).HasForeignKey<TicketRating>(x => x.TicketMessageId).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.RatedByUser).WithMany().HasForeignKey(x => x.RatedByUserId).OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
