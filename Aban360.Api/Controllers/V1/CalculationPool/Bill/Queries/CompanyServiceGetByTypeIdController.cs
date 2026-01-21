using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/company-service-by-typeid")]
    public class CompanyServiceGetByTypeIdController : BaseController
    {
        private readonly ICompanyServiceGetByTypeIdHandler _companyServiceGetByTypeIdHandler;
        public CompanyServiceGetByTypeIdController(ICompanyServiceGetByTypeIdHandler companyServiceGetByTypeIdHandler)
        {
            _companyServiceGetByTypeIdHandler = companyServiceGetByTypeIdHandler;
            _companyServiceGetByTypeIdHandler.NotNull(nameof(companyServiceGetByTypeIdHandler));
        }

        [HttpPost, HttpGet]
        [Route("all/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int id, CancellationToken cancellationToken)
        {
            ICollection<NumericDictionary> companyServices = await _companyServiceGetByTypeIdHandler.Handle(id, cancellationToken);
            return Ok(companyServices);
        }
    }
}
