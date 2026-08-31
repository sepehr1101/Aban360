using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class CustomerLegalSummaryByZoneAndUsageHandler : ICustomerLegalSummaryByZoneAndUsageHandler
    {
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        private string _title = ReportLiterals.CustomerLegalSummary;
        public CustomerLegalSummaryByZoneAndUsageHandler(ICustomerInfoQueryService customerInfoQueryService)
        {
            _customerInfoQueryService = customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));
        }

        public async Task<ReportOutput<CustomerLegalSummaryHeaderOutputDto, CustomerLegalSummaryByZoneAndUsageDataOutputDto>> Handle(CustomerLegalSummaryByZoneAndUsageInputDto input, CancellationToken cancellationToken)
        {
            IEnumerable<CustomerLegalSummaryByZoneAndUsageDataOutputDto> data = await _customerInfoQueryService.GetSummary(input);
            string finalTitle = $"{_title} - {ReportLiterals.ByUsageAndZone}";
            CustomerLegalSummaryHeaderOutputDto header = new()
            {
                CustomerCount = data?.Sum(d => d.InValidLegalCount+d.ValidLegalCount+d.InValidNaturalCount + d.ValidNaturalCount + d.InvalidCount) ?? 0,
                ValidLegalCount = data?.Sum(d => d.ValidLegalCount) ?? 0,
                InValidLegalCount = data?.Sum(d => d.InValidLegalCount) ?? 0,
                ValidNaturalCount = data?.Sum(d => d.ValidLegalCount) ?? 0,
                InValidNaturalCount = data?.Sum(d => d.InValidLegalCount) ?? 0,
                InvalidCount = data?.Sum(d => d.InvalidCount) ?? 0,
                RecordCount = data?.Count() ?? 0,
                Title = _title,
            };
            return new ReportOutput<CustomerLegalSummaryHeaderOutputDto, CustomerLegalSummaryByZoneAndUsageDataOutputDto>(_title, header, data);
        }
    }
}
