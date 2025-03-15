using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/water-resource")]
    public class WaterResourceCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterResourceCreateHandler _waterResourceCreateHandler;
        public WaterResourceCreateController(
            IUnitOfWork uow,
            IWaterResourceCreateHandler waterResourceCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterResourceCreateHandler = waterResourceCreateHandler;
            _waterResourceCreateHandler.NotNull(nameof(waterResourceCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterResourceCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterResourceCreateDto createDto, CancellationToken cancellationToken)
        {
            await _waterResourceCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
