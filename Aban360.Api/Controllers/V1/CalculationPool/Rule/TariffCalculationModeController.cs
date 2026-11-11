using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule
{
    [Route("v1/tariff-calculation-mode")]
    public class TariffCalculationModeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffCalculationModeCreateHandler _tariffCalculationModeCreateHandler;
        private readonly ITariffCalculationModeUpdateHandler _tariffCalculationModeUpdateHandler;
        private readonly ITariffCalculationModeDeleteHandler _tariffCalculationModeDeleteHandler;
        private readonly ITariffCalculationModeGetSingleHandler _tariffCalculationModeGetSingleHandler;
        private readonly ITariffCalculationModeGetAllHandler _tariffCalculationModeGetAllHandler;

        public TariffCalculationModeController(
            IUnitOfWork uow,
            ITariffCalculationModeCreateHandler tariffCalculationModeCreateHandler,
            ITariffCalculationModeUpdateHandler tariffCalculationModeUpdateHandler,
            ITariffCalculationModeDeleteHandler tariffCalculationModeDeleteHandler,
            ITariffCalculationModeGetSingleHandler tariffCalculationModeGetSingleHandler,
            ITariffCalculationModeGetAllHandler tariffCalculationModeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffCalculationModeCreateHandler = tariffCalculationModeCreateHandler;
            _tariffCalculationModeCreateHandler.NotNull(nameof(tariffCalculationModeCreateHandler));

            _tariffCalculationModeUpdateHandler = tariffCalculationModeUpdateHandler;
            _tariffCalculationModeUpdateHandler.NotNull(nameof(tariffCalculationModeUpdateHandler));

            _tariffCalculationModeDeleteHandler = tariffCalculationModeDeleteHandler;
            _tariffCalculationModeDeleteHandler.NotNull(nameof(tariffCalculationModeDeleteHandler));

            _tariffCalculationModeGetSingleHandler = tariffCalculationModeGetSingleHandler;
            _tariffCalculationModeGetSingleHandler.NotNull(nameof(tariffCalculationModeGetSingleHandler));

            _tariffCalculationModeGetAllHandler = tariffCalculationModeGetAllHandler;
            _tariffCalculationModeGetAllHandler.NotNull(nameof(tariffCalculationModeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCalculationModeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TariffCalculationModeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _tariffCalculationModeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCalculationModeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TariffCalculationModeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _tariffCalculationModeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCalculationModeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] TariffCalculationModeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _tariffCalculationModeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCalculationModeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(TariffCalculationModeEnum id, CancellationToken cancellationToken)
        {
            TariffCalculationModeGetDto tariffCalculationModes = await _tariffCalculationModeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(tariffCalculationModes);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffCalculationModeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<TariffCalculationModeGetDto> tariffCalculationModes = await _tariffCalculationModeGetAllHandler.Handle(cancellationToken);
            return Ok(tariffCalculationModes);
        }
    }
}
