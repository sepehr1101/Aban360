using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/company-service")]
    public class CompanyServiceDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceDeleteHandler _companyServiceDeleteHandler;
        public CompanyServiceDeleteController(
            IUnitOfWork uow,
            ICompanyServiceDeleteHandler companyServiceDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _companyServiceDeleteHandler = companyServiceDeleteHandler;
            _companyServiceDeleteHandler.NotNull(nameof(companyServiceDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CompanyServiceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _companyServiceDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
