using Aban360.ClaimPool.Application.Features.Land.Services;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/guild")]
    public class GuildController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IGuildService _service;
        private readonly IGuildByIdQueryHandler _guildByIdQueryHandler;
        private readonly IGuildQueryHandler _guildQueryHandler;
        public GuildController(IUnitOfWork uow, IGuildService service, IGuildByIdQueryHandler guildByIdQueryHandler, IGuildQueryHandler guildQueryHandler)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
            guildByIdQueryHandler.NotNull(nameof(guildByIdQueryHandler));
            _guildByIdQueryHandler = guildByIdQueryHandler;
            guildQueryHandler.NotNull(nameof(guildQueryHandler));
            _guildQueryHandler = guildQueryHandler;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GuildCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] GuildCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GuildUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] GuildUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GuildDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] GuildDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NumericDictionary>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            //GuildGetDto guild = await _service.GetSingleAsync(id, cancellationToken);
            NumericDictionary result = await _guildByIdQueryHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            //ICollection<GuildGetDto> guild = await _service.GetAllAsync(cancellationToken);
            IEnumerable<NumericDictionary> guild = await _guildQueryHandler.Handle(cancellationToken);
            return Ok(guild);
        }
    }
}
