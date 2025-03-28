using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
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
        public virtual DbSet<SubModule> SubModules { get; set; }
        public virtual DbSet<Endpoint> Endpoints { get; set; }

        public virtual DbSet<TokenFailureType> TokenFailureTypes { get; set; }

        public virtual DbSet<UsageLevel1> UsageLevels { get; set; }
        public virtual DbSet<UsageLevel2> UsageLevel2s { get; set; }
        public virtual DbSet<UsageLevel3> UsageLevel3s { get; set; }
        public virtual DbSet<UsageLevel4> UsageLevel4s { get; set; }
        public virtual DbSet<UserLeave> UserLeaves{ get; set; }
        public virtual DbSet<UserWorkday> UserWorkdays{ get; set; }
        public virtual DbSet<OfficialHoliday> OfficialHolidays{ get; set; }
    }
}
