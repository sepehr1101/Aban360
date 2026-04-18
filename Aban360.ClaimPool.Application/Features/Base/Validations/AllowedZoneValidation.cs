using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;

namespace Aban360.ClaimPool.Application.Features.Base.Validations
{
    public class AllowedZoneValidation
    {
        private readonly ICommonZoneService _commonZoneService;
        public AllowedZoneValidation(ICommonZoneService commonZoneService)
        {
            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task Validation(int zoneId,IAppUser appUser)
        {
            IEnumerable<int> allowedZoneIds = await _commonZoneService.GetMyZoneIds(appUser);
            if (!allowedZoneIds.Contains(zoneId))
            {
                throw new AccessZoneException(ExceptionLiterals.NotAccessZone);
            }
        }
    }
}
