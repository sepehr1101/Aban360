using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Queries
{
    [Route("v1/construction-type")]
    public class ConstructionTypeGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IConstructionTypeGetAllHandler _getAllHandler;
        public ConstructionTypeGetAllController(
            IUnitOfWork uow,
            IConstructionTypeGetAllHandler getAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _getAllHandler = getAllHandler;
            _getAllHandler.NotNull(nameof(_getAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ConstructionTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ConstructionTypeGetDto> result = await _getAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
