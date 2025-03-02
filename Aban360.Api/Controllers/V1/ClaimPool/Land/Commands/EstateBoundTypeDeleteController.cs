using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/estate-bound-type")]
    public class EstateBoundTypeDeleteController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateBoundTypeDeleteHandler _deleteHandler;
        public EstateBoundTypeDeleteController(
            IUnitOfWork uow,
            IEstateBoundTypeDeleteHandler deleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _deleteHandler = deleteHandler;
            _deleteHandler.NotNull(nameof(_deleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateBoundTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EstateBoundTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _deleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
