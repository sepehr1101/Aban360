using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-flat")]
    public class RequestFlatCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestFlatCreateHandler _requestFlatCreateHandler;
        public RequestFlatCreateController(
            IUnitOfWork uow,
            IRequestFlatCreateHandler requestFlatCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestFlatCreateHandler = requestFlatCreateHandler;
            _requestFlatCreateHandler.NotNull(nameof(requestFlatCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlatRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] FlatRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestFlatCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
