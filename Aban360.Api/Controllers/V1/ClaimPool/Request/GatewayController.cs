using Aban360.ClaimPool.Application.Features.Request.Services;
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

namespace Aban360.Api.Controllers.V1.ClaimPool.Request
{
    [Route("v1/gateway")]
    public class GatewayController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGatewayService _service;

        public GatewayController(IUnitOfWork uow, IGatewayService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GatewayCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] GatewayCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] GatewayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GatewayDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] GatewayDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GatewayGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var gateways = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(gateways);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<GatewayGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var gateways = await _service.GetAllAsync(cancellationToken);
            return Ok(gateways);
        }
    }
}
