using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/individual-discount-type")]
    public class IndividualDiscountTypeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualDiscountTypeDeleteHandler _individualDiscountTypeDeleteHandler;
        public IndividualDiscountTypeDeleteController(
            IUnitOfWork uow,
            IIndividualDiscountTypeDeleteHandler individualDiscountTypeDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualDiscountTypeDeleteHandler = individualDiscountTypeDeleteHandler;
            _individualDiscountTypeDeleteHandler.NotNull(nameof(individualDiscountTypeDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDiscountTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualDiscountTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _individualDiscountTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
