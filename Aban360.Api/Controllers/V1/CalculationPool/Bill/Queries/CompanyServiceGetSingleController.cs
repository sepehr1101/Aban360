using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/company-service")]
    public class CompanyServiceGetSingleController : BaseController
    {
        private readonly ICompanyServiceGetSingleHandler _companyServiceGetSingleHandler;
        public CompanyServiceGetSingleController(ICompanyServiceGetSingleHandler companyServiceGetSingleHandler)
        {
            _companyServiceGetSingleHandler = companyServiceGetSingleHandler;
            _companyServiceGetSingleHandler.NotNull(nameof(companyServiceGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var companyServices = await _companyServiceGetSingleHandler.Handle(id, cancellationToken);
            return Ok(companyServices);
        }
    }
}
