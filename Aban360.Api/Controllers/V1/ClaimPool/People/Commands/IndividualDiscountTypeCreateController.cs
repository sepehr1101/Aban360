using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/individual-discount-type")]
    public class IndividualDiscountTypeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualDiscountTypeCreateHandler _individualDiscountTypeCreateHandler;
        public IndividualDiscountTypeCreateController(
            IUnitOfWork uow,
            IIndividualDiscountTypeCreateHandler individualDiscountTypeCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualDiscountTypeCreateHandler = individualDiscountTypeCreateHandler;
            _individualDiscountTypeCreateHandler.NotNull(nameof(individualDiscountTypeCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDiscountTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _individualDiscountTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
