using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.WasteWater.Queries
{
    [Route("v1/siphon-type")]
    public class SiphonTypeGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISiphonTypeGetAllHandler _siphonTypeHandler;
        public SiphonTypeGetAllController(
            IUnitOfWork uow,
            ISiphonTypeGetAllHandler siphonTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _siphonTypeHandler = siphonTypeHandler;
            _siphonTypeHandler.NotNull(nameof(siphonTypeHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SiphonTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<SiphonTypeGetDto> siphonTypes = await _siphonTypeHandler.Handle(cancellationToken);
            return Ok(siphonTypes);
        }
    }
}
