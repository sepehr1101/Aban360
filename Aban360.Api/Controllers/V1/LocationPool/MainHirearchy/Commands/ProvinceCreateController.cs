using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Commands
{
    [Route("v1/province")]
    public class ProvinceCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProvinceCreateHandler _provinceCreateHandler;
        public ProvinceCreateController(
            IUnitOfWork uow,
            IProvinceCreateHandler provinceCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _provinceCreateHandler = provinceCreateHandler;
            _provinceCreateHandler.NotNull(nameof(provinceCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProvinceCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ProvinceCreateDto createDto, CancellationToken cancellationToken)
        {
            await _provinceCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
