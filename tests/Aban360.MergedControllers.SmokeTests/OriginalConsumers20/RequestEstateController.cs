// Frozen original consumer; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.Draft;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers20.Aban360.Api.Controllers.V1.ClaimPool.Draft
{
    [Route("v1/request-estate")]
    public class RequestEstateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestEstateCreateHandler _requestEstateCreateHandler;
        private readonly IRequestEstateUpdateHandler _requestEstateUpdateHandler;
        private readonly IRequestEstateDeleteHandler _requestEstateDeleteHandler;

        public RequestEstateController(
            IUnitOfWork uow,
            IRequestEstateCreateHandler requestEstateCreateHandler,
            IRequestEstateUpdateHandler requestEstateUpdateHandler,
            IRequestEstateDeleteHandler requestEstateDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestEstateCreateHandler = requestEstateCreateHandler;
            _requestEstateCreateHandler.NotNull(nameof(requestEstateCreateHandler));

            _requestEstateUpdateHandler = requestEstateUpdateHandler;
            _requestEstateUpdateHandler.NotNull(nameof(requestEstateUpdateHandler));

            _requestEstateDeleteHandler = requestEstateDeleteHandler;
            _requestEstateDeleteHandler.NotNull(nameof(requestEstateDeleteHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EstateRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestEstateUpdateHandler.Handle(CurrentUser,updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EstateRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestEstateDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
