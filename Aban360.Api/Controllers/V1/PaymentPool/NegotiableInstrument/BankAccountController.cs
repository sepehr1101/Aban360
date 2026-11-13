using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument
{
    [Route("v1/bank-account")]
    public class BankAccountController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IBankAccountService _service;

        public BankAccountController(IUnitOfWork uow, IBankAccountService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankAccountCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] BankAccountCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankAccountUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] BankAccountUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankAccountDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] BankAccountDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankAccountGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var bankAccounts = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(bankAccounts);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<BankAccountGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var bankAccounts = await _service.GetAllAsync(cancellationToken);
            return Ok(bankAccounts);
        }
    }
}
