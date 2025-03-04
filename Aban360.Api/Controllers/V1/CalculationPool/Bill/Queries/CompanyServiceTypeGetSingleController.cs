using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/company-service-type")]
    public class CompanyServiceTypeGetSingleController : BaseController
    {
        private readonly ICompanyServiceTypeGetSingleHandler _companyServiceTypeGetSingleHandler;
        public CompanyServiceTypeGetSingleController(ICompanyServiceTypeGetSingleHandler companyServiceTypeGetSingleHandler)
        {
            _companyServiceTypeGetSingleHandler = companyServiceTypeGetSingleHandler;
            _companyServiceTypeGetSingleHandler.NotNull(nameof(companyServiceTypeGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var companyServiceTypes = await _companyServiceTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(companyServiceTypes);
        }
    }
}
