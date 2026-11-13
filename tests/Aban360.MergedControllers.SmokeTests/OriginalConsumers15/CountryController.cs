// Frozen consumer before service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers15.Aban360.Api.Controllers.V1.LocationPool.MainHierarchy
{
    [Route("v1/country")]
    public class CountryController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICountryCreateHandler _countryCreateHandler;
        private readonly ICountryUpdateHandler _countryUpdateHandler;
        private readonly ICountryDeleteHandler _countryDeleteHandler;
        private readonly ICountryGetSingleHandler _countryGetSingleHandler;
        private readonly ICountryGetAllHandler _countryGetAllHandler;

        public CountryController(
            IUnitOfWork uow,
            ICountryCreateHandler countryCreateHandler,
            ICountryUpdateHandler countryUpdateHandler,
            ICountryDeleteHandler countryDeleteHandler,
            ICountryGetSingleHandler countryGetSingleHandler,
            ICountryGetAllHandler countryGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _countryCreateHandler = countryCreateHandler;
            _countryCreateHandler.NotNull(nameof(countryCreateHandler));

            _countryUpdateHandler = countryUpdateHandler;
            _countryUpdateHandler.NotNull(nameof(countryUpdateHandler));

            _countryDeleteHandler = countryDeleteHandler;
            _countryDeleteHandler.NotNull(nameof(countryDeleteHandler));

            _countryGetSingleHandler = countryGetSingleHandler;
            _countryGetSingleHandler.NotNull(nameof(countryGetSingleHandler));

            _countryGetAllHandler = countryGetAllHandler;
            _countryGetAllHandler.NotNull(nameof(countryGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CountryCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CountryCreateDto createDto, CancellationToken cancellationToken)
        {
            await _countryCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CountryUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CountryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _countryUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CountryDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CountryDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _countryDeleteHandler.Handle(deleteDto,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CountryGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id,CancellationToken cancellationToken)
        {
            CountryGetDto result = await _countryGetSingleHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CountryGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<CountryGetDto> result = await _countryGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

    }
}
