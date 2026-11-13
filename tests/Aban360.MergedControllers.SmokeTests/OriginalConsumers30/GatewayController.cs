// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.Request;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.ClaimPool.Request
{
    [Route("v1/gateway")]
    public class GatewayController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGatewayCreateHandler _gatewayCreateHandler;
        private readonly IGatewayUpdateHandler _gatewayUpdateHandler;
        private readonly IGatewayDeleteHandler _gatewayDeleteHandler;
        private readonly IGatewayGetSingleHandler _gatewayGetSingleHandler;
        private readonly IGatewayGetAllHandler _gatewayGetAllHandler;

        public GatewayController(
            IUnitOfWork uow,
            IGatewayCreateHandler gatewayCreateHandler,
            IGatewayUpdateHandler gatewayUpdateHandler,
            IGatewayDeleteHandler gatewayDeleteHandler,
            IGatewayGetSingleHandler gatewayGetSingleHandler,
            IGatewayGetAllHandler gatewayGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _gatewayCreateHandler = gatewayCreateHandler;
            _gatewayCreateHandler.NotNull(nameof(gatewayCreateHandler));

            _gatewayUpdateHandler = gatewayUpdateHandler;
            _gatewayUpdateHandler.NotNull(nameof(gatewayUpdateHandler));

            _gatewayDeleteHandler = gatewayDeleteHandler;
            _gatewayDeleteHandler.NotNull(nameof(gatewayDeleteHandler));

            _gatewayGetSingleHandler = gatewayGetSingleHandler;
            _gatewayGetSingleHandler.NotNull(nameof(gatewayGetSingleHandler));

            _gatewayGetAllHandler = gatewayGetAllHandler;
            _gatewayGetAllHandler.NotNull(nameof(gatewayGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GatewayCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] GatewayCreateDto createDto, CancellationToken cancellationToken)
        {
            await _gatewayCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] GatewayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _gatewayUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GatewayDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] GatewayDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _gatewayDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GatewayGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var gateways = await _gatewayGetSingleHandler.Handle(id, cancellationToken);
            return Ok(gateways);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<GatewayGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var gateways = await _gatewayGetAllHandler.Handle(cancellationToken);
            return Ok(gateways);
        }
    }
}
