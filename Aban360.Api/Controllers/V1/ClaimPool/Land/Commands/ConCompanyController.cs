using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.Land.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/con-company")]
    public class ConCompanyController : BaseController
    {
        private readonly IConCompanyService _conCompanyService;
        private readonly IConCompanyPersonnelService _conCompanyPersonnelService;
        public ConCompanyController(IConCompanyService conCompanyService, IConCompanyPersonnelService conCompanyPersonnelService)
        {
            _conCompanyService = conCompanyService;
            _conCompanyService.NotNull(nameof(conCompanyService));
            _conCompanyPersonnelService = conCompanyPersonnelService;
            _conCompanyPersonnelService.NotNull(nameof(conCompanyPersonnelService));
        }

        [HttpPost, HttpGet]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConCompanyInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] ConCompanyInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            await _conCompanyService.InsertAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConCompanyUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ConCompanyUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _conCompanyService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(int id, CancellationToken cancellationToken)
        {
            await _conCompanyService.RemoveAsync(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ConCompanyGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<ConCompanyGetDto> result = await _conCompanyService.GetAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConCompanyGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            ConCompanyGetDto result = await _conCompanyService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDictionary(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _conCompanyService.GetDictionaryAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("dictionary/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDictionaryByBillId(string billId, CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _conCompanyService.GetByBillIdDictionaryAsync(billId, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("personnel-insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConCompanyPersonnelInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertPersonnel([FromBody] ConCompanyPersonnelInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            await _conCompanyPersonnelService.InsertAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("personnel-update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConCompanyPersonnelUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePersonnel([FromBody] ConCompanyPersonnelUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _conCompanyPersonnelService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("personnel-remove")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ConCompanyPersonnelRemoveInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemovePersonnel([FromBody] ConCompanyPersonnelRemoveInputDto inputDto, CancellationToken cancellationToken)
        {
            await _conCompanyPersonnelService.RemoveAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("personnel-get/{companyId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ConCompanyPersonnelDetailOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonnels(int companyId, CancellationToken cancellationToken)
        {
            IEnumerable<ConCompanyPersonnelDetailOutputDto> result = await _conCompanyPersonnelService.GetAsync(companyId, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("personnel-dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonnelsDictionary(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _conCompanyPersonnelService.GetDictionaryAsync(cancellationToken);
            return Ok(result);
        }
    }
}
