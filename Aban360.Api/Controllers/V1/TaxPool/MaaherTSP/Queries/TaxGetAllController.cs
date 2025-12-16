using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.TaxPool.MaaherTSP.Queries
{
    [Route("v1/tax")]
    public class TaxGetAllController : BaseController
    {
        private readonly IMaliatMaaherWrapperGetAllHandler _maaherWrapperGetAllHandler;
        public TaxGetAllController(IMaliatMaaherWrapperGetAllHandler maaherWrapperGetAllHandler)
        {
            _maaherWrapperGetAllHandler = maaherWrapperGetAllHandler;
            _maaherWrapperGetAllHandler.NotNull(nameof(maaherWrapperGetAllHandler));
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MaliatMaaherWrapperGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> All(CancellationToken cancellationToken)
        {
            IEnumerable<MaliatMaaherWrapperGetDto> result = await _maaherWrapperGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
