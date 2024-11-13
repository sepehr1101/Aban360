using System;
using System.Collections.Generic;
using Aban360.UserPool.Persistence.Features.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Scaffold;

public partial class Aban360Context : DbContext
{
    public Aban360Context()
    {
    }

    public Aban360Context(DbContextOptions<Aban360Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Captcha> Captchas { get; set; }

    public virtual DbSet<CaptchaDisplayMode> CaptchaDisplayModes { get; set; }

    public virtual DbSet<CaptchaLanguage> CaptchaLanguages { get; set; }

    public virtual DbSet<DeepLog> DeepLogs { get; set; }

    public virtual DbSet<OperationType> OperationTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    public virtual DbSet<VersionInfo> VersionInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Encrypt=False;Database=Aban360;Integrated Security=false;User ID=admin;Password=pspihp;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Captcha>(entity =>
        {
            entity.ToTable("Captcha");

            entity.Property(e => e.BackColor)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EncryptionKey).HasMaxLength(1023);
            entity.Property(e => e.FontName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ForeColor)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.HiddenInputName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.HiddenTokenName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Identifier)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InputClass)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InputName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InputPlaceholder).HasMaxLength(255);
            entity.Property(e => e.InputTemplate).HasMaxLength(255);
            entity.Property(e => e.Noise)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NonceKey).HasMaxLength(1023);
            entity.Property(e => e.RateLimitMessage).HasMaxLength(1023);
            entity.Property(e => e.RefreshButtonClass)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ValidationMessage).HasMaxLength(1023);
            entity.Property(e => e.ValidationMessageClass)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.CaptchaDisplayMode).WithMany(p => p.Captchas)
                .HasForeignKey(d => d.CaptchaDisplayModeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaptchaDisplayMode_REFERS_Captcha_CaptchaDisplayModeId");

            entity.HasOne(d => d.CaptchaLanguage).WithMany(p => p.Captchas)
                .HasForeignKey(d => d.CaptchaLanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaptchaLanguage_REFERS_Captcha_CaptchaLanguageId");
        });

        modelBuilder.Entity<CaptchaDisplayMode>(entity =>
        {
            entity.ToTable("CaptchaDisplayMode");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(31)
                .IsUnicode(false);
            entity.Property(e => e.Tite).HasMaxLength(31);
        });

        modelBuilder.Entity<CaptchaLanguage>(entity =>
        {
            entity.ToTable("CaptchaLanguage");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(31)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(31);
        });

        modelBuilder.Entity<DeepLog>(entity =>
        {
            entity.ToTable("DeepLog");

            entity.Property(e => e.InsertDateTime).HasColumnType("datetime");
            entity.Property(e => e.Ip)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.PrimaryDb).HasMaxLength(255);
            entity.Property(e => e.PrimaryId).HasMaxLength(63);
            entity.Property(e => e.PrimaryTable).HasMaxLength(255);

            entity.HasOne(d => d.OperationType).WithMany(p => p.DeepLogs)
                .HasForeignKey(d => d.OperationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperationType_REFERS_DeepLog_OperationTypeId");
        });

        modelBuilder.Entity<OperationType>(entity =>
        {
            entity.ToTable("OperationType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Css).HasMaxLength(1023);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Hash).HasMaxLength(1023);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ_User_Username").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DisplayName).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.LatestLoginDateTime).HasColumnType("datetime");
            entity.Property(e => e.LockTimespan).HasColumnType("datetime");
            entity.Property(e => e.Mobile)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.ToTable("UserLogin");

            entity.Property(e => e.AppVersion).HasMaxLength(15);
            entity.Property(e => e.FirstStepDateTime).HasColumnType("datetime");
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.LogoutDateTime).HasColumnType("datetime");
            entity.Property(e => e.TwoStepCode)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TwoStepExpireDateTime).HasColumnType("datetime");
            entity.Property(e => e.TwoStepInsertDateTime).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(255);
            entity.Property(e => e.WrongPassword).HasMaxLength(1023);

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_REFERS_UserLogin_UserId");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Hash).HasMaxLength(1023);

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_REFERS_UserRole_RoleId");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_REFERS_UserRole_UserId");
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable("UserToken");

            entity.Property(e => e.AccessTokenHash).HasMaxLength(1023);
            entity.Property(e => e.RefreshTokenExpiresDateTime).HasColumnType("datetime");
            entity.Property(e => e.RefreshTokenIdHash).HasMaxLength(1023);
            entity.Property(e => e.RefreshTokenIdHashSource).HasMaxLength(1023);

            entity.HasOne(d => d.User).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_REFERS_UserToken_UserId");
        });

        modelBuilder.Entity<VersionInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("VersionInfo");

            entity.HasIndex(e => e.Version, "UC_Version")
                .IsUnique()
                .IsClustered();

            entity.Property(e => e.AppliedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1024);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
