using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Application.Features.Db70.Services;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Db70
{
    [Route("v1/bill-return-cause")]
    public class BillReturnCauseController : BaseController
    {
        private readonly IBillReturnCauseService _billReturnCauseService;
        public BillReturnCauseController(IBillReturnCauseService billReturnCauseService)
        {
            _billReturnCauseService = billReturnCauseService;
            _billReturnCauseService.NotNull(nameof(billReturnCauseService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(BillReturnCauseCreateDto createDto, CancellationToken cancellationToken)
        {
            await _billReturnCauseService.CreateAsync(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(BillReturnCauseDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _billReturnCauseService.DeleteAsync(deleteDto, CurrentUser, cancellationToken);
            return Ok(deleteDto);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(BillReturnCauseUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _billReturnCauseService.UpdateAsync(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<BillReturnCauseGetDto> result = await _billReturnCauseService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(short id, CancellationToken cancellationToken)
        {
            SearchShortInputDto inputDto = id;
            BillReturnCauseGetDto result = await _billReturnCauseService.GetAsync(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
