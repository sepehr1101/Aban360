using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/company-service")]
    public class CompanyServiceCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceCreateHandler _companyServiceCreateHandler;
        public CompanyServiceCreateController(
            IUnitOfWork uow,
            ICompanyServiceCreateHandler companyServiceCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _companyServiceCreateHandler = companyServiceCreateHandler;
            _companyServiceCreateHandler.NotNull(nameof(companyServiceCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CompanyServiceCreateDto createDto, CancellationToken cancellationToken)
        {
            await _companyServiceCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
