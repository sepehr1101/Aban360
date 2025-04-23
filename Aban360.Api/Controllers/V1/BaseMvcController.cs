using Aban360.Common.ApplicationUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1
{
    [EnableCors("CorsPolicy")]
    [Authorize]
    public abstract class BaseMvcController : Controller
    {
        public IAppUser CurrentUser
        {
            get
            {
                return new AppUser(User);
            }
        }
    }
}
