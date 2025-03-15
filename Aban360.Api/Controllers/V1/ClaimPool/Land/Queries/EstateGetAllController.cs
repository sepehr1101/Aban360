using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/estate")]
    public class EstateGetAllController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateGetAllHandler _getAllHandler;
        public EstateGetAllController(
            IUnitOfWork uow,
            IEstateGetAllHandler getAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _getAllHandler = getAllHandler;
            _getAllHandler.NotNull(nameof(_getAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EstateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<EstateGetDto> estate = await _getAllHandler.Handle(cancellationToken);
            return Ok(estate);
        }
    }
}
