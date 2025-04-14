using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-individual-discount-type")]
    public class RequestIndividualDiscountTypeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualDiscountTypeDeleteHandler _requestIndividualDiscountTypeDeleteHandler;
        public RequestIndividualDiscountTypeDeleteController(
            IUnitOfWork uow,
            IRequestIndividualDiscountTypeDeleteHandler requestIndividualDiscountTypeDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualDiscountTypeDeleteHandler = requestIndividualDiscountTypeDeleteHandler;
            _requestIndividualDiscountTypeDeleteHandler.NotNull(nameof(requestIndividualDiscountTypeDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestIndividualDiscountTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] RequestIndividualDiscountTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestIndividualDiscountTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
