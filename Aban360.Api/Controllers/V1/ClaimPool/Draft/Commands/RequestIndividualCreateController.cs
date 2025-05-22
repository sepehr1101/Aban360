using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-individual")]
    public class RequestIndividualCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualCreateHandler _requestIndividualCreateHandler;
        public RequestIndividualCreateController(
            IUnitOfWork uow,
            IRequestIndividualCreateHandler requestIndividualCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestIndividualCreateHandler = requestIndividualCreateHandler;
            _requestIndividualCreateHandler.NotNull(nameof(requestIndividualCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestIndividualCreateHandler.Handle(CurrentUser,createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
