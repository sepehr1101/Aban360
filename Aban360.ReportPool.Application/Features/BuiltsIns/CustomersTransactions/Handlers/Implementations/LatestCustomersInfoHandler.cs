using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class LatestCustomersInfoHandler : ILatestCustomersInfoHandler
    {
        private readonly ILatestCustomersInfoQueryService _queryService;
        public LatestCustomersInfoHandler(ILatestCustomersInfoQueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ReportOutput<LatestCustomersInfoHeaderOutputDto, LatestCustomersInfoDataOutputDto>> Handle(LatestCustomersInfoInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<LatestCustomersInfoHeaderOutputDto, LatestCustomersInfoDataOutputDto> result = await _queryService.Get(inputDto);
            return result;
        }
    }
}
