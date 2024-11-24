using Aban360.UserPool.Domain.Features.Auth.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Contexts.Implementation
{
    public partial class Aban360Context
    {
        public virtual DbSet<CaptchaLanguage> CaptchaLanguages { get; set; } = null!;
        public virtual DbSet<CaptchaDisplayMode> CaptchaDisplayMode { get; set; } = null!;
        public virtual DbSet<Captcha> Captchas { get; set; } = null!;

        public virtual DbSet<InvalidLoginReason> InvalidLoginReasons { get; set; }
        public virtual DbSet<LogoutReason> LogoutReasons { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }
    }
}
