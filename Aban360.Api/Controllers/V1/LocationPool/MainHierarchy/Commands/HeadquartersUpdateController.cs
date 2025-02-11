using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/headquarters")]
    public class HeadquartersUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IHeadquarterUpdateHandler _headquarterUpdateHandler;
        public HeadquartersUpdateController(
            IUnitOfWork uow,
            IHeadquarterUpdateHandler headquarterUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _headquarterUpdateHandler = headquarterUpdateHandler;
            _headquarterUpdateHandler.NotNull(nameof(headquarterUpdateHandler));
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] HeadquarterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _headquarterUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
