using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/estate")]
    public class EstatDeleteController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateDeleteHandler _deleteHandler;
        public EstatDeleteController(
            IUnitOfWork uow,
            IEstateDeleteHandler deleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _deleteHandler = deleteHandler;
            _deleteHandler.NotNull(nameof(_deleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] EstateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _deleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
