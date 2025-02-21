using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Commands
{
    [Route("v1/siphon-diameter")]
    public class SiphonDiameterCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonDiameterCreateHandler _siphonDiameterHandler;
        public SiphonDiameterCreateController(
            IUnitOfWork uow,
            ISiphonDiameterCreateHandler siphonDiameterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonDiameterHandler = siphonDiameterHandler;
            _siphonDiameterHandler.NotNull(nameof(siphonDiameterHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SiphonDiameterCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SiphonDiameterCreateDto createDto, CancellationToken cancellationToken)
        {
            await _siphonDiameterHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
