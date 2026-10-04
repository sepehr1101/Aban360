using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.AutoReading.Exceptions;
using Aban360.MeterPool.Application.Features.AutoReading.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.AutoReading.Dtos.Queries;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Aban360.Api.Controllers.V1.MeterPool.AutoReading.Queries
{
    [Route("v1/auto-reading")]
    public class FlowMeterReportGetController : BaseController
    {
        private readonly IFlowMeterReportGetHandler _flowMeterReportGetHandler;

        public FlowMeterReportGetController(IFlowMeterReportGetHandler flowMeterReportGetHandler)
        {
            _flowMeterReportGetHandler = flowMeterReportGetHandler;
            _flowMeterReportGetHandler.NotNull(nameof(flowMeterReportGetHandler));
        }

        [HttpPost("flow-meter-report")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlowMeterReportGetDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromBody] FlowMeterReportInputDto inputDto, CancellationToken cancellationToken)
        {
            try
            {
                FlowMeterReportGetDto result = await _flowMeterReportGetHandler.Handle(inputDto, cancellationToken);
                return Ok(result);
            }
            catch (NonUltrasonicMeterException exception)
            {
                return ClientError(exception.Message);
            }
            catch (CustomValidationException exception)
            {
                return ClientError(exception.Message);
            }
            catch (HttpRequestException)
            {
                return UpstreamError(StatusCodes.Status502BadGateway, "ارتباط با سرویس قرائت خودکار ناموفق بود.");
            }
            catch (JsonException)
            {
                return UpstreamError(StatusCodes.Status502BadGateway, "پاسخ سرویس قرائت خودکار معتبر نیست.");
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                return UpstreamError(StatusCodes.Status504GatewayTimeout, "مهلت دریافت پاسخ از سرویس قرائت خودکار به پایان رسید.");
            }
        }

        [HttpPost("flow-meter-report/sti")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FlowMeterReportGetDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetSti([FromBody] FlowMeterReportInputDto inputDto, CancellationToken cancellationToken)
        {
            try
            {
                int reportCode = 800;
                FlowMeterReportGetDto result = await _flowMeterReportGetHandler.Handle(inputDto, cancellationToken);
                JsonReportId reportId = await JsonOperation.ExportToJsonFlat(result, cancellationToken, reportCode);
                return Ok(reportId);
            }
            catch (NonUltrasonicMeterException exception)
            {
                return ClientError(exception.Message);
            }
            catch (CustomValidationException exception)
            {
                return ClientError(exception.Message);
            }
            catch (HttpRequestException)
            {
                return UpstreamError(StatusCodes.Status502BadGateway, "ارتباط با سرویس قرائت خودکار ناموفق بود.");
            }
            catch (JsonException)
            {
                return UpstreamError(StatusCodes.Status502BadGateway, "پاسخ سرویس قرائت خودکار معتبر نیست.");
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                return UpstreamError(StatusCodes.Status504GatewayTimeout, "مهلت دریافت پاسخ از سرویس قرائت خودکار به پایان رسید.");
            }
        }


        private IActionResult UpstreamError(int statusCode, string message)
        {
            return StatusCode(statusCode, new ApiResponseEnvelope<object>(statusCode, null, null,
                new List<ApiError> { new ApiError(message, statusCode) }));
        }
    }
}
