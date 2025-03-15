using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Commands
{
    [Route("v1/siphon")]
    public class SiphonCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonCreateHandler _siphonHandler;
        public SiphonCreateController(
            IUnitOfWork uow,
            ISiphonCreateHandler siphonHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonHandler = siphonHandler;
            _siphonHandler.NotNull(nameof(siphonHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SiphonCreateDto createDto, CancellationToken cancellationToken)
        {
            await _siphonHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
