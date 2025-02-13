using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Commands
{
    [Route("v1/headquarters")]
    public class HeadquartersDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IHeadquarterDeleteHandler _headquarterDeleteHandler;
        public HeadquartersDeleteController(
            IUnitOfWork uow,
            IHeadquarterDeleteHandler headquarterDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _headquarterDeleteHandler = headquarterDeleteHandler;
            _headquarterDeleteHandler.NotNull(nameof(headquarterDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] HeadquarterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _headquarterDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
