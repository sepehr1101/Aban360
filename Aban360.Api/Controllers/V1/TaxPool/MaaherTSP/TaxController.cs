using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Commands.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Microsoft.AspNetCore.Mvc;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.TaxPool.MaaherTSP
{
    public class TaxController : BaseController
    {
        private readonly INewListCreateHandler _c0_sentInvoiceHandler;
        private readonly ISentInvoiceHandler _c1_sentInvoiceHandler;
        private readonly IMaliatMaaherDetailGetByWrapperIdHandler _c2_maliatMaaherDetailGetHandler;
        private readonly IReportGenerator _c2_reportGenerator;
        private readonly IMaliatMaaherWrapperGetAllHandler _c3_maaherWrapperGetAllHandler;
        public TaxController(INewListCreateHandler c0_sentInvoiceHandler, ISentInvoiceHandler c1_sentInvoiceHandler, IMaliatMaaherDetailGetByWrapperIdHandler c2_maliatMaaherDetailGetHandler, IReportGenerator c2_reportGenerator, IMaliatMaaherWrapperGetAllHandler c3_maaherWrapperGetAllHandler)
        {
            _c0_sentInvoiceHandler = c0_sentInvoiceHandler;
            _c0_sentInvoiceHandler.NotNull(nameof(c0_sentInvoiceHandler));
            _c1_sentInvoiceHandler = c1_sentInvoiceHandler;
            _c1_sentInvoiceHandler.NotNull(nameof(c1_sentInvoiceHandler));
            _c2_maliatMaaherDetailGetHandler = c2_maliatMaaherDetailGetHandler;
            _c2_maliatMaaherDetailGetHandler.NotNull(nameof(c2_maliatMaaherDetailGetHandler));
            _c2_reportGenerator = c2_reportGenerator;
            _c2_reportGenerator.NotNull(nameof(c2_reportGenerator));
            _c3_maaherWrapperGetAllHandler = c3_maaherWrapperGetAllHandler;
            _c3_maaherWrapperGetAllHandler.NotNull(nameof(c3_maaherWrapperGetAllHandler));
        }

        [HttpPost]
        [Route("~/v1/tax/create-list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateList(NewListCreateDto inputDto, CancellationToken cancellationToken)
        {
            int newWrapperId = await _c0_sentInvoiceHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(newWrapperId);
        }

        [HttpPost]
        [Route("~/v1/tax/send-manual")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MaaherResponseNew>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendManual(ICollection<MaaherRequestWrapper_New> inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<MaaherResponseNew> sentInvoiceRecieve = await _c1_sentInvoiceHandler.Handle(inputDto, cancellationToken);
            return Ok(sentInvoiceRecieve);
        }

        [HttpPost]
        [Route("~/v1/tax/confirm/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MaaherResponseNew>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Confirm(int id, CancellationToken cancellationToken)
        {
            IEnumerable<MaaherResponseNew> sentInvoiceRecieve = await _c1_sentInvoiceHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(sentInvoiceRecieve);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/tax/excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SearchByIdInput inputDto, CancellationToken cancellationToken)
        {
            await _c2_reportGenerator.FireAndInform(inputDto, cancellationToken, _c2_maliatMaaherDetailGetHandler.Handle, CurrentUser, ReportLiterals.ClientValidation, connectionId);
            return Ok(inputDto);
        }

        [HttpGet]
        [Route("~/v1/tax/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MaliatMaaherWrapperGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> All(CancellationToken cancellationToken)
        {
            IEnumerable<MaliatMaaherWrapperGetDto> result = await _c3_maaherWrapperGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
