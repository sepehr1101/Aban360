using Aban360.Api.Controllers.V1;
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
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ReportPool.Persistence.Queries.Implementations;
using Aban360.Api.Filters;

namespace BeforeServiceConsumers19.Aban360.Api.Controllers.V1.ClaimPool.People
{
    public class IndividualController : BaseController
    {
        private readonly IUnitOfWork _c0_uow;
        private readonly IIndividualCreateHandler _c0_individualCreateHandler;
        private readonly IIndividualUpdateHandler _c0_individualUpdateHandler;
        private readonly IIndividualDeleteHandler _c0_individualDeleteHandler;
        private readonly IIndividualGetSingleHandler _c0_individualGetSingleHandler;
        private readonly IIndividualGetAllHandler _c0_individualGetAllHandler;
        private readonly IIndividualsInfoGetHandler _c1_individualsSummaryInfoGetHandler;
        private readonly IIndividualSummeryQueryService _c2_individualSummeryQueryService;
        public IndividualController(IUnitOfWork c0_uow, IIndividualCreateHandler c0_individualCreateHandler, IIndividualUpdateHandler c0_individualUpdateHandler, IIndividualDeleteHandler c0_individualDeleteHandler, IIndividualGetSingleHandler c0_individualGetSingleHandler, IIndividualGetAllHandler c0_individualGetAllHandler, IIndividualsInfoGetHandler c1_individualsSummaryInfoGetHandler, IIndividualSummeryQueryService c2_individualSummeryQueryService)
        {
            _c0_uow = c0_uow;
            _c0_uow.NotNull(nameof(c0_uow));
            _c0_individualCreateHandler = c0_individualCreateHandler;
            _c0_individualCreateHandler.NotNull(nameof(c0_individualCreateHandler));
            _c0_individualUpdateHandler = c0_individualUpdateHandler;
            _c0_individualUpdateHandler.NotNull(nameof(c0_individualUpdateHandler));
            _c0_individualDeleteHandler = c0_individualDeleteHandler;
            _c0_individualDeleteHandler.NotNull(nameof(c0_individualDeleteHandler));
            _c0_individualGetSingleHandler = c0_individualGetSingleHandler;
            _c0_individualGetSingleHandler.NotNull(nameof(c0_individualGetSingleHandler));
            _c0_individualGetAllHandler = c0_individualGetAllHandler;
            _c0_individualGetAllHandler.NotNull(nameof(c0_individualGetAllHandler));
            _c1_individualsSummaryInfoGetHandler = c1_individualsSummaryInfoGetHandler;
            _c1_individualsSummaryInfoGetHandler.NotNull(nameof(c1_individualsSummaryInfoGetHandler));
            _c2_individualSummeryQueryService = c2_individualSummeryQueryService;
            _c2_individualSummeryQueryService.NotNull(nameof(c2_individualSummeryQueryService));
        }

        [HttpPost]
        [Route("~/v1/individual/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualCreateDto>), StatusCodes.Status200OK)]
        [ActionName("Create")]
        public async Task<IActionResult> CreateFromIndividual0([FromBody] IndividualCreateDto createDto, CancellationToken cancellationToken)
        {
            await _c0_individualCreateHandler.Handle(createDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/individual/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _c0_individualUpdateHandler.Handle(updateDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("~/v1/individual/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _c0_individualDeleteHandler.Handle(deleteDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/individual/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualGetDto>), StatusCodes.Status200OK)]
        [ActionName("Create")]
        public async Task<IActionResult> CreateFromIndividual0(int id, CancellationToken cancellationToken)
        {
            IndividualGetDto individual = await _c0_individualGetSingleHandler.Handle(id, cancellationToken);
            return Ok(individual);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/individual/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<IndividualGetDto> individual = await _c0_individualGetAllHandler.Handle(cancellationToken);
            return Ok(individual);
        }

        [HttpPost]
        [Route("~/v1/individual/info")] //todo: change url
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<IndividualsInfoDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Info([FromBody] SearchInput searchInput, CancellationToken cancellationToken)
        {
            IEnumerable<IndividualsInfoDto> summary = await _c1_individualsSummaryInfoGetHandler.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }

        [HttpPost]
        [Route("~/v1/individual/owner")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<IndividualSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> OwnerShipSummary([FromBody] SearchInput searchInput)
        {
            IEnumerable<IndividualSummaryDto> summary = await _c2_individualSummeryQueryService.GetOwnerShipSummery(searchInput.Input, (short)IndividualEstateRelationTypeEnum.OwnerShip);
            return Ok(summary);
        }

        [HttpPost]
        [Route("~/v1/individual/stakeholder")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<IndividualSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> StakeHolderSummary([FromBody] SearchInput searchInput)
        {
            IEnumerable<IndividualSummaryDto> summary = await _c2_individualSummeryQueryService.GetStakeHolderSummery(searchInput.Input, (short)IndividualEstateRelationTypeEnum.OwnerShip);
            return Ok(summary);
        }
    }
}
