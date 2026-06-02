using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Requests.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Request.Inputs;
using Aban360.ReportPool.Domain.Features.Request.Outputs;
using Aban360.ReportPool.Persistence.Features.Request.Contracts;

namespace Aban360.ReportPool.Application.Features.Requests.Handlers.Implementations
{
    internal sealed class ReceivedSmdByPaigingHandler : IReceivedSmdByPaigingHandler
    {
        private readonly IReceivedSmsDetailQueryService _receivedSmsDetailQueryService;
        public ReceivedSmdByPaigingHandler(IReceivedSmsDetailQueryService receivedSmsDetailQueryService)
        {
            _receivedSmsDetailQueryService = receivedSmsDetailQueryService;
            _receivedSmsDetailQueryService.NotNull(nameof(receivedSmsDetailQueryService));
        }

        public async Task<ReportOutput<ReceivedSmsHeaderOutputDto, ReceivedSmsDataOutputDto>> Handle(ReceivedSmsInputDto inputDto, CancellationToken cancellationToken)
        {
            string title = ReportLiterals.ReceivedSms;
            IEnumerable<ReceivedSmsDataOutputDto> receivedSmsList = await _receivedSmsDetailQueryService.Get(inputDto);

            ReceivedSmsHeaderOutputDto header = new(title, receivedSmsList.Count());
            ReportOutput<ReceivedSmsHeaderOutputDto, ReceivedSmsDataOutputDto> result = new(title, header, receivedSmsList);
            return result;
        }
    }
}
