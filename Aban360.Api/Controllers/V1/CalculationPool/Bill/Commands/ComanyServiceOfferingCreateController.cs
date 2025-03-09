using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/company-service-offering")]
    public class CompanyServiceOfferingCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceOfferingCreateHandler _companyServiceOfferingCreateHandler;
        public CompanyServiceOfferingCreateController(
            IUnitOfWork uow,
            ICompanyServiceOfferingCreateHandler companyServiceOfferingCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _companyServiceOfferingCreateHandler = companyServiceOfferingCreateHandler;
            _companyServiceOfferingCreateHandler.NotNull(nameof(companyServiceOfferingCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceOfferingCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CompanyServiceOfferingCreateDto createDto, CancellationToken cancellationToken)
        {
            await _companyServiceOfferingCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}