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
using Aban360.Api.Controllers.V1;
namespace BeforeSalesReportsConsumers.Aban360.Api.Controllers.V1.CalculationPool.Sale{
    [Route("v1/tanker-water-distance-tariff")]
    public class TankerWaterDistanceTariffController : BaseController
    {
        private readonly ITankerWaterDistanceTariffCreateHadler _c0_createHadler;
        private readonly ITankerWaterDistanceTariffDeleteHadler _c1_deleteHadler;
        private readonly ITankerWaterDistanceTariffUpdateHadler _c2_updateHadler;
        private readonly ITankerWaterDistanceTariffService _tankerWaterDistanceTariffService;
        public TankerWaterDistanceTariffController(ITankerWaterDistanceTariffCreateHadler c0_createHadler, ITankerWaterDistanceTariffDeleteHadler c1_deleteHadler, ITankerWaterDistanceTariffUpdateHadler c2_updateHadler, ITankerWaterDistanceTariffService tankerWaterDistanceTariffService)
        {
            _c0_createHadler = c0_createHadler;
            _c0_createHadler.NotNull(nameof(c0_createHadler));
            _c1_deleteHadler = c1_deleteHadler;
            _c1_deleteHadler.NotNull(nameof(c1_deleteHadler));
            _c2_updateHadler = c2_updateHadler;
            _c2_updateHadler.NotNull(nameof(c2_updateHadler));
            _tankerWaterDistanceTariffService = tankerWaterDistanceTariffService;
            _tankerWaterDistanceTariffService.NotNull(nameof(tankerWaterDistanceTariffService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterDistanceTariffInputDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("TankerWaterDistanceTariffCreate")]
        public async Task<IActionResult> Create([FromBody] TankerWaterDistanceTariffInputDto createDto, CancellationToken cancellationToken)
        {
            await _c0_createHadler.Handle(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        [EndpointAuthorization("TankerWaterDistanceTariffDelete")]
        public async Task<IActionResult> Delete([FromBody] SearchById input, CancellationToken cancellationToken)
        {
            await _c1_deleteHadler.Handle(input, CurrentUser, cancellationToken);
            return Ok(input);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterDistanceTariffInputDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("TankerWaterDistanceTariffUpdate")]
        public async Task<IActionResult> Update([FromBody] TankerWaterDistanceTariffInputDto UpdateDto, CancellationToken cancellationToken)
        {
            await _c2_updateHadler.Handle(UpdateDto, CurrentUser, cancellationToken);
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

        [HttpPost, HttpGet]
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
