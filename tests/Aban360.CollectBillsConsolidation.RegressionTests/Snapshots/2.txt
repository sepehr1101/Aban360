using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Queries.Implementations
{
    internal sealed class CollectBillsDetailWithLastStepGetHadler : ICollectBillsDetailWithLastStepGetHadler
    {
        private readonly ICollectBillsDetailQueryService _detailQueryService;
        private string _title = ReportLiterals.CollectBillsReport;
        public CollectBillsDetailWithLastStepGetHadler(ICollectBillsDetailQueryService detailQueryService)
        {
            _detailQueryService = detailQueryService;
            _detailQueryService.NotNull(nameof(detailQueryService));
        }

        public async Task<ReportOutput<CollectBillsDetailWithLastStepHeaderOutputDto, CollectBillsDetailWithLastStepDataOutputDto>> Handle(CollectBillsDetialReportInputDto inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<CollectBillsDetailWithLastStepDataOutputDto> data = await _detailQueryService.Get(inputDto);
            CollectBillsDetailWithLastStepHeaderOutputDto header = new()
            {
                Title = _title,
                RecordCount = data?.Count() ?? 0
            };

            ReportOutput<CollectBillsDetailWithLastStepHeaderOutputDto, CollectBillsDetailWithLastStepDataOutputDto> result = new(_title, header, data);
            return result;
        }
    }
}
