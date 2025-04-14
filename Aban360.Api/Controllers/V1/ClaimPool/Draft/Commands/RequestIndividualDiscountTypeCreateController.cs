using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-individual-discount-type")]
    public class RequestIndividualDiscountTypeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualDiscountTypeCreateHandler _requestIndividualDiscountTypeCreateHandler;
        public RequestIndividualDiscountTypeCreateController(
            IUnitOfWork uow,
            IRequestIndividualDiscountTypeCreateHandler requestIndividualDiscountTypeCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualDiscountTypeCreateHandler = requestIndividualDiscountTypeCreateHandler;
            _requestIndividualDiscountTypeCreateHandler.NotNull(nameof(requestIndividualDiscountTypeCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RequestIndividualDiscountTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RequestIndividualDiscountTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestIndividualDiscountTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
