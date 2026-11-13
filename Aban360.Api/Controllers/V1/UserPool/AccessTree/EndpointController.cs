using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree
{
    [Route("v1/endpoint")]
    public class EndpointController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEndpointCreateHandler _endpointCreateHandler;
        private readonly IEndpointUpdateHandler _endpointUpdateHandler;
        private readonly IEndpointDeleteHandler _endpointDeleteHandler;
        private readonly IEndpointGetSingleHandler _endpointGetSingleHandler;
        private readonly IEndpointGetAllHandler _endpointGetAllHandler;

        public EndpointController(
            IUnitOfWork uow,
            IEndpointCreateHandler endpointCreateHandler,
            IEndpointUpdateHandler endpointUpdateHandler,
            IEndpointDeleteHandler endpointDeleteHandler,
            IEndpointGetSingleHandler endpointGetSingleHandler,
            IEndpointGetAllHandler endpointGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _endpointCreateHandler = endpointCreateHandler;
            _endpointCreateHandler.NotNull(nameof(endpointCreateHandler));

            _endpointUpdateHandler = endpointUpdateHandler;
            _endpointUpdateHandler.NotNull(nameof(endpointUpdateHandler));

            _endpointDeleteHandler = endpointDeleteHandler;
            _endpointDeleteHandler.NotNull(nameof(endpointDeleteHandler));

            _endpointGetSingleHandler = endpointGetSingleHandler;
            _endpointGetSingleHandler.NotNull(nameof(endpointGetSingleHandler));

            _endpointGetAllHandler = endpointGetAllHandler;
            _endpointGetAllHandler.NotNull(nameof(endpointGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EndpointCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EndpointCreateDto createDto, CancellationToken cancellationToken)
        {
            await _endpointCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPatch, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EndpointUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EndpointUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _endpointUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpDelete, HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EndpointDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EndpointDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _endpointDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet,HttpPost]
        [Route("single")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EndpointGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Single(int id, CancellationToken cancellationToken)
        {
            var endpoint = await _endpointGetSingleHandler.Handle(id, cancellationToken);
            return Ok(endpoint);
        }

        [HttpGet,HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EndpointGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var endpoint = await _endpointGetAllHandler.Handle(cancellationToken);
            return Ok(endpoint);
        }
    }
}
