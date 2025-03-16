using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/estate-water-resource")]
    public class EstateWaterResourceDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateWaterResourceDeleteHandler _estateWaterResourceDeleteHandler;
        public EstateWaterResourceDeleteController(
            IUnitOfWork uow,
            IEstateWaterResourceDeleteHandler estateWaterResourceDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _estateWaterResourceDeleteHandler = estateWaterResourceDeleteHandler;
            _estateWaterResourceDeleteHandler.NotNull(nameof(estateWaterResourceDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateWaterResourceDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EstateWaterResourceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _estateWaterResourceDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
