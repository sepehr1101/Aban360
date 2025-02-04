using System.Security.Claims;

namespace Aban360.Common.ApplicationUser
{
    public class AppUser : ClaimsPrincipal, IAppUser
    {
        public AppUser(ClaimsPrincipal principal)
            : base(principal)
        {
        }

        public Guid UserId
        {
            get
            {
                return Guid.Parse(FindFirst(ClaimTypes.UserData)?.Value);
            }
        }
        public string Username
        {
            get
            {
                return FindFirst(ClaimTypes.Name).Value;
            }
        }
        public string FullName
        {
            get
            {
                return FindFirst(ClaimTypes.GivenName).Value;
            }
        }
        public ICollection<string> Roles
        {
            get
            {
                return FindAll(u => u.Type.Trim() == ClaimTypes.Role)
                    .Select(u => u.Value)
                    .ToList();
            }
        }
        public bool IsAdmin
        {
            get
            {
                return FindAll(u => u.Type.Trim() == ClaimTypes.Role)
                     .Select(u => u.Value)
                     .ToList().Any(r => r.Trim() == "admin");
            }
        }
    }
}
