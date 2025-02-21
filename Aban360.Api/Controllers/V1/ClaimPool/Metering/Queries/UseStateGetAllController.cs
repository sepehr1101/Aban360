using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/use-state")]
    public class UseStateGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUseStateGetAllHandler _useEstateHandler;
        public UseStateGetAllController(
            IUnitOfWork uow,
            IUseStateGetAllHandler useEstateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _useEstateHandler = useEstateHandler;
            _useEstateHandler.NotNull(nameof(useEstateHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UseStateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var useEstates = await _useEstateHandler.Handle(cancellationToken);
            return Ok(useEstates);
        }
    }
}
