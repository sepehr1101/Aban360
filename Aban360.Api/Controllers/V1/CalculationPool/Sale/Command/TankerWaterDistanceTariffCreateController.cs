using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Command
{
    [Route("v1/tanker-water-distance-tariff")]
    public class TankerWaterDistanceTariffCreateController : BaseController
    {
        private readonly ITankerWaterDistanceTariffCreateHadler _createHadler;
        public TankerWaterDistanceTariffCreateController(ITankerWaterDistanceTariffCreateHadler createHadler)
        {
            _createHadler = createHadler;
            _createHadler.NotNull(nameof(createHadler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterDistanceTariffInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TankerWaterDistanceTariffInputDto createDto, CancellationToken cancellationToken)
        {
            await _createHadler.Handle(createDto, CurrentUser, cancellationToken);

            return Ok(createDto);
        }
    }
}
