using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/use-state")]
    public class UseStateDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUseStateDeleteHandler _useEstateHandler;
        public UseStateDeleteController(
            IUnitOfWork uow,
            IUseStateDeleteHandler useEstateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _useEstateHandler = useEstateHandler;
            _useEstateHandler.NotNull(nameof(useEstateHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UseStateDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UseStateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _useEstateHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
