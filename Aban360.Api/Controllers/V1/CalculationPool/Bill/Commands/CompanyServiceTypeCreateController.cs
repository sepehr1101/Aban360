using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/company-service-type")]
    public class CompanyServiceTypeCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceTypeCreateHandler _companyServiceTypeCreateHandler;
        public CompanyServiceTypeCreateController(
            IUnitOfWork uow,
            ICompanyServiceTypeCreateHandler companyServiceTypeCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _companyServiceTypeCreateHandler = companyServiceTypeCreateHandler;
            _companyServiceTypeCreateHandler.NotNull(nameof(companyServiceTypeCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CompanyServiceTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _companyServiceTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
