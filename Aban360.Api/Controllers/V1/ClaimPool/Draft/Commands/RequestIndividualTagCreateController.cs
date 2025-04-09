using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request_individual_tag")]
    public class RequestIndividualTagCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualTagCreateHandler _requestIndividualTagCreateHandler;
        public RequestIndividualTagCreateController(
            IUnitOfWork uow,
            IRequestIndividualTagCreateHandler requestIndividualTagCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualTagCreateHandler = requestIndividualTagCreateHandler;
            _requestIndividualTagCreateHandler.NotNull(nameof(requestIndividualTagCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualTagRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestIndividualTagCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
