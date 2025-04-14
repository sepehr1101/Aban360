using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/intividual-discount-type")]
    public class IndividualDiscountTypeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualDiscountTypeUpdateHandler _individualDiscountTypeUpdateHandler;
        public IndividualDiscountTypeUpdateController(
            IUnitOfWork uow,
            IIndividualDiscountTypeUpdateHandler individualDiscountTypeUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualDiscountTypeUpdateHandler = individualDiscountTypeUpdateHandler;
            _individualDiscountTypeUpdateHandler.NotNull(nameof(individualDiscountTypeUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDiscountTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualDiscountTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _individualDiscountTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
