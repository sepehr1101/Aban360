using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/estate-bound-type")]
    public class EstateBoundTypeGetAllController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateBoundTypeGetAllHandler _getAllHandler;
        public EstateBoundTypeGetAllController(
            IUnitOfWork uow,
            IEstateBoundTypeGetAllHandler getAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _getAllHandler = getAllHandler;
            _getAllHandler.NotNull(nameof(_getAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EstateBoundTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var estateBoundType = await _getAllHandler.Handle(cancellationToken);
            return Ok(estateBoundType);
        }
    }
}
