// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.Land;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/handover")]
    public class HandoverController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IHandoverCreateHandler _handoverCreateHandler;
        private readonly IHandoverUpdateHandler _handoverUpdateHandler;
        private readonly IHandoverDeleteHandler _handoverDeleteHandler;
        private readonly IHandoverGetSingleHandler _handoverGetSingleHandler;
        private readonly IHandoverGetAllHandler _handoverGetAllHandler;
        private readonly IHandoverHandler _handoverHandler;

        public HandoverController(
            IUnitOfWork uow,
            IHandoverCreateHandler handoverCreateHandler,
            IHandoverUpdateHandler handoverUpdateHandler,
            IHandoverDeleteHandler handoverDeleteHandler,
            IHandoverGetSingleHandler handoverGetSingleHandler,
            IHandoverGetAllHandler handoverGetAllHandler,
            IHandoverHandler handoverHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _handoverCreateHandler = handoverCreateHandler;
            _handoverCreateHandler.NotNull(nameof(handoverCreateHandler));

            _handoverUpdateHandler = handoverUpdateHandler;
            _handoverUpdateHandler.NotNull(nameof(handoverUpdateHandler));

            _handoverDeleteHandler = handoverDeleteHandler;
            _handoverDeleteHandler.NotNull(nameof(handoverDeleteHandler));

            _handoverGetSingleHandler = handoverGetSingleHandler;
            _handoverGetSingleHandler.NotNull(nameof(handoverGetSingleHandler));

            _handoverGetAllHandler = handoverGetAllHandler;
            _handoverGetAllHandler.NotNull(nameof(handoverGetAllHandler));

            _handoverHandler = handoverHandler;
            _handoverHandler.NotNull(nameof(handoverHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HandoverCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] HandoverCreateDto createDto, CancellationToken cancellationToken)
        {
            await _handoverCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HandoverUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] HandoverUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _handoverUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HandoverDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] HandoverDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _handoverDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HandoverGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var handovers = await _handoverGetSingleHandler.Handle(id, cancellationToken);
            return Ok(handovers);
        }

        [HttpPost, HttpGet]
        [Route("all-2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<HandoverGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var handovers = await _handoverGetAllHandler.Handle(cancellationToken);
            return Ok(handovers);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<HandoverQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            ICollection<HandoverQueryDto> handovers = (await _handoverHandler.Handle(cancellationToken)).ToList();
            return Ok(handovers);
        }
    }
}
