using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Implementations
{
    public class ConsumerSummaryAddhoc : IConsumerSummaryAddhoc
    {
        private readonly IConsumerSummaryGetHandler _consumerSummaryGetHandler;
        public ConsumerSummaryAddhoc(IConsumerSummaryGetHandler consumerSummaryGetHandler)
        {
            _consumerSummaryGetHandler = consumerSummaryGetHandler;
            _consumerSummaryGetHandler.NotNull(nameof(consumerSummaryGetHandler));
        }

        public async Task<ConsumerSummaryDto> Handle(string billId, CancellationToken cancellationToken)
        {
            var consumerSummary = await _consumerSummaryGetHandler.Handle(billId, cancellationToken);
            return consumerSummary;
        }
    }
}
