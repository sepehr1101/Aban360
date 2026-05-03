using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Command
{
    [Route("v1/tanker-water")]
    public class TankerWaterController : BaseController
    {
        private readonly ITankerInsertHandler _tankerInserHandler;
        private readonly ITankerRemoveHandler _tankerRemoveHandler;
        private readonly ISmsOldHandler _smsOldHandler;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public TankerWaterController(
            ITankerRemoveHandler tankerRemoveHandler,
            ITankerInsertHandler tankerInserHandler,
            ISmsOldHandler smsOldHandler,
            IBackgroundJobClient backgroundJobClient)
        {
            _tankerRemoveHandler = tankerRemoveHandler;
            _tankerRemoveHandler.NotNull(nameof(tankerRemoveHandler));

            _tankerInserHandler = tankerInserHandler;
            _tankerInserHandler.NotNull(nameof(tankerInserHandler));

            _smsOldHandler = smsOldHandler;
            _smsOldHandler.NotNull(nameof(smsOldHandler));

            _backgroundJobClient = backgroundJobClient;
            _backgroundJobClient.NotNull(nameof(backgroundJobClient));
        }

        [HttpGet, HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerWaterCalculationOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] TankerInsertInputDto input, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            TankerWaterCalculationOutputDto result = await _tankerInserHandler.Handle(input, userCode, cancellationToken);
            string text = string.Format(SmsTemplates.TankerWater, input.Consumption, input.Distance, result.Final);
            if (input.HasSms && !string.IsNullOrWhiteSpace(input.MobileNumber))
            {
                _backgroundJobClient.Enqueue(() => _smsOldHandler.Send(input.MobileNumber, text, Guid.NewGuid()));
            }
            return Ok(result);
        }


        [HttpGet, HttpPost]
        [Route("remove")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TankerRemoveInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromBody] TankerRemoveInputDto input, CancellationToken cancellationToken)
        {
            int userCode = UserService.GetUserCode(CurrentUser.Username);
            await _tankerRemoveHandler.Handle(input, userCode, cancellationToken);
            return Ok(input);
        }
    }
}
