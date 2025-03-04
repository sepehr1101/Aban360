using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/company-service")]
    public class CompanyServiceUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceUpdateHandler _companyServiceUpdateHandler;
        public CompanyServiceUpdateController(
            IUnitOfWork uow,
            ICompanyServiceUpdateHandler companyServiceUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _companyServiceUpdateHandler = companyServiceUpdateHandler;
            _companyServiceUpdateHandler.NotNull(nameof(companyServiceUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] CompanyServiceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _companyServiceUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
