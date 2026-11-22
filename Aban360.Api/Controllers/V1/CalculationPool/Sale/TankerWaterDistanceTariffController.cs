using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Api.Filters;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Application.Features.Sale.Services;
using Aban360.Api.Controllers.V1.CalculationPool.Sale;
using ITankerWaterDistanceTariffService = global::Aban360.CalculationPool.Application.Features.Sale.Services.ITankerWaterDistanceTariffService;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale
{
    [Route("v1/tanker-water-distance-tariff")]
    public class TankerWaterDistanceTariffController : BaseController
    {
        private readonly ITankerWaterDistanceTariffService _tankerWaterDistanceTariffService;
        public TankerWaterDistanceTariffController(ITankerWaterDistanceTariffService tankerWaterDistanceTariffService)
        {
            _tankerWaterDistanceTariffService = tankerWaterDistanceTariffService;
            _tankerWaterDistanceTariffService.NotNull(nameof(tankerWaterDistanceTariffService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterDistanceTariffInputDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("TankerWaterDistanceTariffCreate")]
        public async Task<IActionResult> Create([FromBody] TankerWaterDistanceTariffInputDto createDto, CancellationToken cancellationToken)
        {
            await _tankerWaterDistanceTariffService.TankerWaterDistanceTariffCreateAsync(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        [EndpointAuthorization("TankerWaterDistanceTariffDelete")]
        public async Task<IActionResult> Delete([FromBody] SearchById input, CancellationToken cancellationToken)
        {
            await _tankerWaterDistanceTariffService.TankerWaterDistanceTariffDeleteAsync(input, CurrentUser, cancellationToken);
            return Ok(input);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterDistanceTariffInputDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("TankerWaterDistanceTariffUpdate")]
        public async Task<IActionResult> Update([FromBody] TankerWaterDistanceTariffInputDto UpdateDto, CancellationToken cancellationToken)
        {
            await _tankerWaterDistanceTariffService.TankerWaterDistanceTariffUpdateAsync(UpdateDto, CurrentUser, cancellationToken);
            return Ok(UpdateDto);
        }

        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<TankerWaterDistanceTariffOutputDto>>), StatusCodes.Status200OK)]
        [ActionName("GetAll")]
        public async Task<IActionResult> GetAllFromTankerWaterDistanceTariffGetAll(CancellationToken cancellationToken)
        {
            IEnumerable<TankerWaterDistanceTariffOutputDto> result = await _tankerWaterDistanceTariffService.TankerWaterDistanceTariffGetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterDistanceTariffOutputDto>), StatusCodes.Status200OK)]
        [ActionName("GetAll")]
        public async Task<IActionResult> GetAllFromTankerWaterDistanceTariffGet([FromBody] SearchById input, CancellationToken cancellationToken)
        {
            TankerWaterDistanceTariffOutputDto result = await _tankerWaterDistanceTariffService.TankerWaterDistanceTariffGetAsync(input, cancellationToken);
            return Ok(result);
        }
    }
}
