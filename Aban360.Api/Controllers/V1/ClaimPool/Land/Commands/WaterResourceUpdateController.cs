using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/water-resource")]
    public class WaterResourceUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterResourceUpdateHandler _waterResourceUpdateHandler;
        public WaterResourceUpdateController(
            IUnitOfWork uow,
            IWaterResourceUpdateHandler waterResourceUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterResourceUpdateHandler = waterResourceUpdateHandler;
            _waterResourceUpdateHandler.NotNull(nameof(waterResourceUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] WaterResourceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _waterResourceUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
