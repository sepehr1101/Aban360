using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Aban360.Common.Categories.ApplicationUser
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
        public int UserCode
        {
            get
            {
                return Convert.ToInt32(this.FindFirst(CustomClaimTypes.UserCode).Value);
            }
        }
        public string DisplayName
        {
            get
            {
                return FindFirst(CustomClaimTypes.DisplayName).Value;
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
        public bool Is1522User
        {
            get
            {
                return Claims.Where(c => c.Type.Trim() == "roleId" && c.Value == "2").Any();
            }
        }
    }
}
