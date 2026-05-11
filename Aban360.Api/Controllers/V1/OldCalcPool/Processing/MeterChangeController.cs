using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using DNTPersianUtils.Core;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/meter-change")]
    public class MeterChangeController : BaseController
    {
        private readonly IGenerateBillHandler _generateBillHandler;
        private readonly IMeterChangeCreateHandler _meterChangeCreateHandler;
        private readonly IMeterChangeGetHandler _meterChangeGetHandler;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public MeterChangeController(
            IGenerateBillHandler generateBillHandler,
            IMeterChangeCreateHandler meterChangeCreateHandler,
            IMeterChangeGetHandler meterChangeGetHandler,
            IBackgroundJobClient backgroundJobClient)
        {
            _generateBillHandler = generateBillHandler;
            _generateBillHandler.NotNull(nameof(generateBillHandler));

            _meterChangeCreateHandler = meterChangeCreateHandler;
            _meterChangeCreateHandler.NotNull(nameof(meterChangeCreateHandler));

            _meterChangeGetHandler = meterChangeGetHandler;
            _meterChangeGetHandler.NotNull(nameof(meterChangeGetHandler));

            _backgroundJobClient = backgroundJobClient;
            _backgroundJobClient.NotNull(nameof(backgroundJobClient));
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NewBillOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddMeterChange([FromBody] MeterChangeInputDto inputDto, CancellationToken cancellationToken)
        {
            GenerateBillInputDto generateBillInputdto = new(inputDto.BillId, inputDto.MeterNumber, DateTime.Now.ToShortPersianDateString(), inputDto.IsConfirm);
            NewBillOutputDto billResult = await _generateBillHandler.Handle(generateBillInputdto, CurrentUser, cancellationToken);
            if (inputDto.IsConfirm)
            {
                _backgroundJobClient.Enqueue(() => _meterChangeCreateHandler.Handle(inputDto, cancellationToken));
            }
            return Ok(billResult);
        }


        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterChangeInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeterChange([FromBody] SearchInput inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<MeterChangeInfoOutputDto> result = await _meterChangeGetHandler.Handle(inputDto.Input, cancellationToken);
            return Ok(result);
        }
    }
}
