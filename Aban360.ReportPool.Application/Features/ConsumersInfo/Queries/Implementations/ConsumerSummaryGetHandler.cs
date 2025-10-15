using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    public class ConsumerSummaryGetHandler : IConsumerSummaryGetHandler
    {
        private readonly IConsumerSummaryQueryService _consumerSummaryService;
        public ConsumerSummaryGetHandler(IConsumerSummaryQueryService consumerSummaryService)
        {
            _consumerSummaryService = consumerSummaryService;
            _consumerSummaryService.NotNull(nameof(consumerSummaryService));
        }

        public async Task<ConsumerSummaryDto> Handle(string billId, CancellationToken cancellationToken)
        {
            var consumerSummary = await _consumerSummaryService.GetInfo(billId);
            consumerSummary.TotalUnitWater = consumerSummary.UnitDomesticWater + consumerSummary.UnitCommercialWater + consumerSummary.UnitOtherWater;
            consumerSummary.TotalUnitSewage = consumerSummary.UnitDomesticSewage + consumerSummary.UnitCommercialSewage + consumerSummary.UnitOtherSewage;
          
            return consumerSummary;
        }
    }
}
