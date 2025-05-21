using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-estate")]
    public class RequestEstateCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestEstateCreateHandler _requestEstateCreateHandler;
        public RequestEstateCreateController(
            IUnitOfWork uow,
            IRequestEstateCreateHandler requestEstateCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestEstateCreateHandler = requestEstateCreateHandler;
            _requestEstateCreateHandler.NotNull(nameof(requestEstateCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EstateRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestEstateCreateHandler.Handle(CurrentUser,createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
