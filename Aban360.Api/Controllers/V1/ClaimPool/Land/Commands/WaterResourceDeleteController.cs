using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/water-resource")]
    public class WaterResourceDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterResourceDeleteHandler _waterResourceDeleteHandler;
        public WaterResourceDeleteController(
            IUnitOfWork uow,
            IWaterResourceDeleteHandler waterResourceDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterResourceDeleteHandler = waterResourceDeleteHandler;
            _waterResourceDeleteHandler.NotNull(nameof(waterResourceDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterResourceDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterResourceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _waterResourceDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
