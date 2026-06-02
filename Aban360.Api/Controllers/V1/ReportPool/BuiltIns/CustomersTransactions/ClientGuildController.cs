using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/client-guild")]
    public class ClientGuildController : BaseController
    {
        private readonly IClientGuildDetailHandler _clientGuildDetailHandler;
        private readonly IClientGuildSummaryHandler _clientGuildSummaryHandler;
        private readonly IReportGenerator _reportGenerator;
        public ClientGuildController(
            IClientGuildDetailHandler clientGuildDetailHandler,
            IClientGuildSummaryHandler clientGuildSummaryHandler,
            IReportGenerator reportGenerator)
        {
            _clientGuildDetailHandler = clientGuildDetailHandler;
            _clientGuildDetailHandler.NotNull(nameof(clientGuildDetailHandler));

            _clientGuildSummaryHandler= clientGuildSummaryHandler;
            _clientGuildSummaryHandler.NotNull(nameof(clientGuildSummaryHandler));  

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("detail-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ClientGuildDetailHeaderOutputDto, ClientGuildDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailRaw(ClientGuildInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ClientGuildDetailHeaderOutputDto, ClientGuildDetailDataOutputDto> ClientGuildDetail = await _clientGuildDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(ClientGuildDetail);
        }

        [HttpPost, HttpGet]
        [Route("detail-excel/{connectionId}")]
        public async Task<IActionResult> GetDetailExcel(string connectionId, ClientGuildInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _clientGuildDetailHandler.Handle, CurrentUser, ReportLiterals.ClientGuildDetail, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("detail-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailStiReport(ClientGuildInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2100;
            ReportOutput<ClientGuildDetailHeaderOutputDto, ClientGuildDetailDataOutputDto> result = await _clientGuildDetailHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
        
        [HttpPost, HttpGet]
        [Route("summary-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ClientGuildSummaryHeaderOutputDto, ClientGuildSummaryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetsummaryRaw(ClientGuildInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ClientGuildSummaryHeaderOutputDto, ClientGuildSummaryDataOutputDto> ClientGuildsummary = await _clientGuildSummaryHandler.Handle(inputDto, cancellationToken);
            return Ok(ClientGuildsummary);
        }

        [HttpPost, HttpGet]
        [Route("summary-excel/{connectionId}")]
        public async Task<IActionResult> GetsummaryExcel(string connectionId, ClientGuildInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _clientGuildSummaryHandler.Handle, CurrentUser, ReportLiterals.ClientGuildSummary, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("summary-sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetsummaryStiReport(ClientGuildInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2101;
            ReportOutput<ClientGuildSummaryHeaderOutputDto, ClientGuildSummaryDataOutputDto> result = await _clientGuildSummaryHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
