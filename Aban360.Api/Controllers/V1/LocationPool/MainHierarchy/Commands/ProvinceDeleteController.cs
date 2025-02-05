using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/province")]
    public class ProvinceDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProvinceDeleteHandler _provinceDeleteHandler;
        public ProvinceDeleteController(
            IUnitOfWork uow,
            IProvinceDeleteHandler provinceDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _provinceDeleteHandler = provinceDeleteHandler;
            _provinceDeleteHandler.NotNull(nameof(provinceDeleteHandler));
        }


        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProvinceDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ProvinceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _provinceDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
