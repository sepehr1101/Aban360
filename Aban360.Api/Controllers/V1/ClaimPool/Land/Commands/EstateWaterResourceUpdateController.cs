using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/estate-water-resource")]
    public class EstateWaterResourceUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateWaterResourceUpdateHandler _estateWaterResourceUpdateHandler;
        public EstateWaterResourceUpdateController(
            IUnitOfWork uow,
            IEstateWaterResourceUpdateHandler estateWaterResourceUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _estateWaterResourceUpdateHandler = estateWaterResourceUpdateHandler;
            _estateWaterResourceUpdateHandler.NotNull(nameof(estateWaterResourceUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] EstateWaterResourceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _estateWaterResourceUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
