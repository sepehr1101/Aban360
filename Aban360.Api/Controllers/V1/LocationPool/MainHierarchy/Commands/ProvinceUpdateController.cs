using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/province")]
    public class ProvinceUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProvinceUpdateHandler _provinceUpdateHandler;
        public ProvinceUpdateController(
            IUnitOfWork uow,
            IProvinceUpdateHandler provinceUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _provinceUpdateHandler = provinceUpdateHandler;
            _provinceUpdateHandler.NotNull(nameof(provinceUpdateHandler));
        }


        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProvinceUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ProvinceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _provinceUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
