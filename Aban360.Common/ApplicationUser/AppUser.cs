using System.Security.Claims;

namespace Aban360.Common.ApplicationUser
{
    public class AppUserManual : IAppUser
    {
        public Guid UserId { get; private set; }
        public string Username { get; private set; }
        public string FullName { get; private set; }
        public ICollection<string> Roles { get; }
        public bool IsAdmin { get; set; }
        public AppUserManual()
        {
            UserId = Guid.Parse("A2DB559D-22BD-4A49-964E-29FDF88D7D8A");
            Username = "2416256";
            FullName = "دستیار رایاب";
        }
    }
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
