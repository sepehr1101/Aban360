using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/contractual-capacity")]
    public class ContractualCapacityController : BaseController
    {
        private readonly IContractualCapacityHandler _contractualCapacity;
        public ContractualCapacityController(IContractualCapacityHandler contractualCapacity)
        {
            _contractualCapacity = contractualCapacity;
            _contractualCapacity.NotNull(nameof(_contractualCapacity));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ContractualCapacityInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto> contractualCapacity = await _contractualCapacity.Handle(inputDto, cancellationToken);
            return Ok(contractualCapacity);
        }
    }
}
