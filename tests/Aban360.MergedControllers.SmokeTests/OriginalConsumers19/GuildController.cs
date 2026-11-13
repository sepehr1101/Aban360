using Aban360.Api.Controllers.V1;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers19.Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/guild")]
    public class GuildController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGuildCreateHandler _guildCreateHandler;
        private readonly IGuildUpdateHandler _guildUpdateHandler;
        private readonly IGuildDeleteHandler _guildDeleteHandler;
        private readonly IGuildGetSingleHandler _guildGetSingleHandler;
        private readonly IGuildByIdQueryHandler _guildByIdQueryHandler;
        private readonly IGuildGetAllHandler _guildGetAllHandler;
        private readonly IGuildQueryHandler _guildQueryHandler;

        public GuildController(
            IUnitOfWork uow,
            IGuildCreateHandler guildCreateHandler,
            IGuildUpdateHandler guildUpdateHandler,
            IGuildDeleteHandler guildDeleteHandler,
            IGuildGetSingleHandler guildGetSingleHandler,
            IGuildByIdQueryHandler guildByIdQueryHandler,
            IGuildGetAllHandler guildGetAllHandler,
            IGuildQueryHandler guildQueryHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _guildCreateHandler = guildCreateHandler;
            _guildCreateHandler.NotNull(nameof(guildCreateHandler));

            _guildUpdateHandler = guildUpdateHandler;
            _guildUpdateHandler.NotNull(nameof(guildUpdateHandler));

            _guildDeleteHandler = guildDeleteHandler;
            _guildDeleteHandler.NotNull(nameof(guildDeleteHandler));

            _guildGetSingleHandler = guildGetSingleHandler;
            _guildGetSingleHandler.NotNull(nameof(guildGetSingleHandler));

            _guildByIdQueryHandler = guildByIdQueryHandler;
            _guildByIdQueryHandler.NotNull(nameof(guildByIdQueryHandler));

            _guildGetAllHandler = guildGetAllHandler;
            _guildGetAllHandler.NotNull(nameof(guildGetAllHandler));

            _guildQueryHandler = guildQueryHandler;
            _guildQueryHandler.NotNull(nameof(guildQueryHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GuildCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] GuildCreateDto createDto, CancellationToken cancellationToken)
        {
            await _guildCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GuildUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] GuildUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _guildUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GuildDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] GuildDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _guildDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NumericDictionary>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            //GuildGetDto guild = await _guildGetSingleHandler.Handle(id, cancellationToken);
            NumericDictionary result = await _guildByIdQueryHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            //ICollection<GuildGetDto> guild = await _guildGetAllHandler.Handle(cancellationToken);
            IEnumerable<NumericDictionary> guild = await _guildQueryHandler.Handle(cancellationToken);
            return Ok(guild);
        }
    }
}
