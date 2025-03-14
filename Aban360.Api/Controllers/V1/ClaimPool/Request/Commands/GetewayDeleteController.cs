using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/geteway")]
    public class GetewayDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGetewayDeleteHandler _getewayDeleteHandler;
        public GetewayDeleteController(
            IUnitOfWork uow,
            IGetewayDeleteHandler getewayDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _getewayDeleteHandler = getewayDeleteHandler;
            _getewayDeleteHandler.NotNull(nameof(getewayDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GetewayDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] GetewayDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _getewayDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
