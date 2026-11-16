using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions;
[Route("v1/water-receivables")]
public class WaterReceivablesController : BaseController
{
    private readonly IWaterReceivablesHandler _handler;
    private readonly IReportGenerator _reportGenerator;

    public WaterReceivablesController(IWaterReceivablesHandler handler, IReportGenerator reportGenerator)
    {
        _handler = handler;
        _handler.NotNull(nameof(handler));
        _reportGenerator = reportGenerator;
        _reportGenerator.NotNull(nameof(reportGenerator));
    }

    /// <summary>
    /// گزارش مانده مطالبات آب‌بها؛ گروه‌بندی: 1 کاربری، 2 ناحیه، 3 منطقه.
    /// مانده ابتدا از 1330/01/01 تا روز قبل از شروع دوره محاسبه می‌شود.
    /// </summary>
    [HttpPost("raw")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<WaterReceivablesHeaderOutputDto, WaterReceivablesDataOutputDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRaw([FromBody] WaterReceivablesInputDto input, CancellationToken cancellationToken)
        => Ok(await _handler.Handle(input, cancellationToken));

    /// <summary>
    /// ایجاد فایل اکسل گزارش مانده مطالبات و اعلام نتیجه از طریق اتصال SignalR.
    /// </summary>
    [HttpPost("excel/{connectionId}")]
    public async Task<IActionResult> GetExcel(string connectionId, [FromBody] WaterReceivablesInputDto input, CancellationToken cancellationToken)
    {
        await _reportGenerator.FireAndInform(input, cancellationToken, _handler.Handle,
            CurrentUser, ReportLiterals.WaterReceivables, connectionId);
        return Ok(input);
    }

    /// <summary>
    /// ایجاد داده JSON گزارش چاپی مانده مطالبات برای قالب STI با کد 2900.
    /// </summary>
    [HttpPost("sti")]
    [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStiReport([FromBody] WaterReceivablesInputDto input, CancellationToken cancellationToken)
    {
        const int reportCode = 2900;
        var result = await _handler.Handle(input, cancellationToken);
        var reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
        return Ok(reportId);
    }
}
