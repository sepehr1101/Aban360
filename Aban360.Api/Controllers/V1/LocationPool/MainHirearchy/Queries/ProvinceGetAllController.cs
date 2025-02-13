using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Queries
{
    [Route("v1/province")]
    public class ProvinceGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProvinceGetAllHandler _provinceGetAllHandler;
        public ProvinceGetAllController(
            IUnitOfWork uow,
            IProvinceGetAllHandler provinceGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _provinceGetAllHandler = provinceGetAllHandler;
            _provinceGetAllHandler.NotNull(nameof(provinceGetAllHandler));
        }


        [HttpGet,HttpPost]
        [Route("All")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ProvinceGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _provinceGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
