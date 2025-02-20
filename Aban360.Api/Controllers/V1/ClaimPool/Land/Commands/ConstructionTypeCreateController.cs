using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/construction-type")]
    public class ConstructionTypeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IConstructionTypeCreateHandler _createHandler;
        public ConstructionTypeCreateController(
            IUnitOfWork uow,
            IConstructionTypeCreateHandler createHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _createHandler = createHandler;
            _createHandler.NotNull(nameof(_createHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConstructionTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ConstructionTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _createHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
