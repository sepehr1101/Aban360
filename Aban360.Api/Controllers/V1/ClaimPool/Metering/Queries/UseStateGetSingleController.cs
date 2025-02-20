using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/use-state")]
    public class UseStateGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUseStateGetSingleHandler _useEstateHandler;
        public UseStateGetSingleController(
            IUnitOfWork uow,
            IUseStateGetSingleHandler useEstateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _useEstateHandler = useEstateHandler;
            _useEstateHandler.NotNull(nameof(useEstateHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UseStateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(UseStateEnum id, CancellationToken cancellationToken)
        {
            var useEstate = await _useEstateHandler.Handle(id, cancellationToken);
            return Ok(useEstate);
        }
    }
}
