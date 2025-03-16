using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/estate-water-resource")]
    public class EstateWaterResourceCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateWaterResourceCreateHandler _estateWaterResourceCreateHandler;
        public EstateWaterResourceCreateController(
            IUnitOfWork uow,
            IEstateWaterResourceCreateHandler estateWaterResourceCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _estateWaterResourceCreateHandler = estateWaterResourceCreateHandler;
            _estateWaterResourceCreateHandler.NotNull(nameof(estateWaterResourceCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateWaterResourceCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EstateWaterResourceCreateDto createDto, CancellationToken cancellationToken)
        {
            await _estateWaterResourceCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
