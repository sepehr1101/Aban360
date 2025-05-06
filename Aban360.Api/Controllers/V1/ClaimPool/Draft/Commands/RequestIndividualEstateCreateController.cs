using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-individual-estate")]
    public class RequestIndividualEstateCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualEstateCreateHandler _requestIndividualEstateCreateHandler;
        public RequestIndividualEstateCreateController(
            IUnitOfWork uow,
            IRequestIndividualEstateCreateHandler requestIndividualEstateCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualEstateCreateHandler = requestIndividualEstateCreateHandler;
            _requestIndividualEstateCreateHandler.NotNull(nameof(requestIndividualEstateCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualEstateRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualEstateRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestIndividualEstateCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
