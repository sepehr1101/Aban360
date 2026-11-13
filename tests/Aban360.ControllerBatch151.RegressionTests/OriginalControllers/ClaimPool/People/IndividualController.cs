using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BeforeBatch151.ClaimPool.People
{
    [Route("v1/individual")]
    public class IndividualController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualCreateHandler _individualCreateHandler;
        private readonly IIndividualUpdateHandler _individualUpdateHandler;
        private readonly IIndividualDeleteHandler _individualDeleteHandler;
        private readonly IIndividualGetSingleHandler _individualGetSingleHandler;
        private readonly IIndividualGetAllHandler _individualGetAllHandler;

        public IndividualController(
            IUnitOfWork uow,
            IIndividualCreateHandler individualCreateHandler,
            IIndividualUpdateHandler individualUpdateHandler,
            IIndividualDeleteHandler individualDeleteHandler,
            IIndividualGetSingleHandler individualGetSingleHandler,
            IIndividualGetAllHandler individualGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualCreateHandler = individualCreateHandler;
            _individualCreateHandler.NotNull(nameof(individualCreateHandler));

            _individualUpdateHandler = individualUpdateHandler;
            _individualUpdateHandler.NotNull(nameof(individualUpdateHandler));

            _individualDeleteHandler = individualDeleteHandler;
            _individualDeleteHandler.NotNull(nameof(individualDeleteHandler));

            _individualGetSingleHandler = individualGetSingleHandler;
            _individualGetSingleHandler.NotNull(nameof(individualGetSingleHandler));

            _individualGetAllHandler = individualGetAllHandler;
            _individualGetAllHandler.NotNull(nameof(individualGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualCreateDto createDto, CancellationToken cancellationToken)
        {
            await _individualCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _individualUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _individualDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(int id, CancellationToken cancellationToken)
        {
            IndividualGetDto individual = await _individualGetSingleHandler.Handle(id, cancellationToken);
            return Ok(individual);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<IndividualGetDto> individual = await _individualGetAllHandler.Handle(cancellationToken);
            return Ok(individual);
        }
    }
}
