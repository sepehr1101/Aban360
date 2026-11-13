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

namespace Aban360.Api.Controllers.V1.ClaimPool.People
{
    public class IndividualTagController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIndividualTagCreateHandler _individualTagCreateHandler;
        private readonly IIndividualTagDeleteHandler _individualTagDeleteHandler;
        private readonly IIndividualTagUpdateHandler _individualTagUpdateHandler;
        private readonly IIndividualTagGetSinglBySearchInputeHandler _individualTagGetSinglBySearchInputeHandler;
        private readonly IIndividualTagGetSingleHandler _individualTagGetSingleHandler;

        public IndividualTagController(
            IUnitOfWork unitOfWork,
            IIndividualTagCreateHandler individualTagCreateHandler,
            IIndividualTagDeleteHandler individualTagDeleteHandler,
            IIndividualTagUpdateHandler individualTagUpdateHandler,
            IIndividualTagGetSinglBySearchInputeHandler individualTagGetSinglBySearchInputeHandler,
            IIndividualTagGetSingleHandler individualTagGetSingleHandler)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.NotNull(nameof(unitOfWork));
            _individualTagCreateHandler = individualTagCreateHandler;
            _individualTagCreateHandler.NotNull(nameof(individualTagCreateHandler));
            _individualTagDeleteHandler = individualTagDeleteHandler;
            _individualTagDeleteHandler.NotNull(nameof(individualTagDeleteHandler));
            _individualTagUpdateHandler = individualTagUpdateHandler;
            _individualTagUpdateHandler.NotNull(nameof(individualTagUpdateHandler));
            _individualTagGetSinglBySearchInputeHandler = individualTagGetSinglBySearchInputeHandler;
            _individualTagGetSinglBySearchInputeHandler.NotNull(nameof(individualTagGetSinglBySearchInputeHandler));
            _individualTagGetSingleHandler = individualTagGetSingleHandler;
            _individualTagGetSingleHandler.NotNull(nameof(individualTagGetSingleHandler));
        }

        [HttpPost]
        [Route("~/individual-tag/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualTagCreateDto createDto, CancellationToken cancellationToken)
        {
            await _individualTagCreateHandler.Handle(createDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpDelete]
        [Route("~/individual-tag/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualTagDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _individualTagDeleteHandler.Handle(deleteDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/individual-tag/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualTagUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _individualTagUpdateHandler.Handle(updateDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpGet]
        [Route("~/individual-tag/search/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualTagGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(string input, CancellationToken cancellationToken)
        {
            IndividualTagGetDto IndividualTag = await _individualTagGetSinglBySearchInputeHandler.Handle(input, cancellationToken);
            return Ok(IndividualTag);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/individual-tag/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<IndividualTagGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            ICollection<IndividualTagGetDto> IndividualTag = await _individualTagGetSingleHandler.Handle(id, cancellationToken);
            return Ok(IndividualTag);
        }
    }
}
