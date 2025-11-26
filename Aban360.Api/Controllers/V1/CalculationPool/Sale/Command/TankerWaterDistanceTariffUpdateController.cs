using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Command
{
    [Route("v1/tanker-water-distance-tariff")]
    public class TankerWaterDistanceTariffUpdateController : BaseController
    {
        private readonly ITankerWaterDistanceTariffUpdateHadler _updateHadler;
        public TankerWaterDistanceTariffUpdateController(ITankerWaterDistanceTariffUpdateHadler updateHadler)
        {
            _updateHadler = updateHadler;
            _updateHadler.NotNull(nameof(updateHadler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterDistanceTariffInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TankerWaterDistanceTariffInputDto UpdateDto, CancellationToken cancellationToken)
        {
            await _updateHadler.Handle(UpdateDto, CurrentUser, cancellationToken);

            return Ok(UpdateDto);
        }
    }
}
