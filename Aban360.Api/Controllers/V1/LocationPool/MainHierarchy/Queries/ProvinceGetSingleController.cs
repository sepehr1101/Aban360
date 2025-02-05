using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/province")]
    public class ProvinceGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProvinceGetSingleHandler _provinceGetSingleHandler;
        public ProvinceGetSingleController(
            IUnitOfWork uow, 
            IProvinceGetSingleHandler provinceGetSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _provinceGetSingleHandler = provinceGetSingleHandler;
            _provinceGetSingleHandler.NotNull(nameof(provinceGetSingleHandler));    
        }

        [HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProvinceGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var result=await _provinceGetSingleHandler.Handle(id,cancellationToken); 
            return Ok(result);
        }
    }
}
