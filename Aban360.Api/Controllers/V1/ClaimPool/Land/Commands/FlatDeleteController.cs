using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/falt")]
    public class FlatDeleteController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IFlatDeleteHandler _flatHandler;
        public FlatDeleteController(
            IUnitOfWork uow,
            IFlatDeleteHandler flatHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _flatHandler = flatHandler;
            _flatHandler.NotNull(nameof(flatHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] FlatDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _flatHandler.Handle(deleteDto,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
