using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Contexts.Implementation
{
    public partial class UserPoolContext
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
        public virtual DbSet<App> Apps { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Controller> Controllers { get; set; }
        public virtual DbSet<Endpoint> Endpoints { get; set; }
    }
}
