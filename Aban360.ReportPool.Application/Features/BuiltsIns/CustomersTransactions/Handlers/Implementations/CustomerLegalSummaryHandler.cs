using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class CustomerLegalSummaryHandler : ICustomerLegalSummaryHandler
    {
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        private string _title = ReportLiterals.CustomerLegalSummary;
        public CustomerLegalSummaryHandler(ICustomerInfoQueryService customerInfoQueryService)
        {
            _customerInfoQueryService = customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));
        }

        public async Task<ReportOutput<CustomerLegalSummaryHeaderOutputDto, CustomerLegalSummaryDataOutputDto>> Handle(CustomerLegalInputDto input, CancellationToken cancellationToken)
        {
            IEnumerable<CustomerLegalSummaryDataOutputDto> data = await _customerInfoQueryService.GetSummary(input);
            CustomerLegalSummaryHeaderOutputDto header = new()
            {
                ZoneCount = input?.ZoneIds?.Count() ?? 0,
                CustomerCount = data?.Sum(d => d.LegalCount + d.NaturalCount + d.InvalidCount) ?? 0,
                LegalCount = data?.Sum(d => d.LegalCount) ?? 0,
                NaturalCount = data?.Sum(d => d.LegalCount) ?? 0,
                InvalidCount = data?.Sum(d => d.InvalidCount) ?? 0,
                RecordCount = data?.Count() ?? 0,
                Title = _title,
            };
            return new ReportOutput<CustomerLegalSummaryHeaderOutputDto, CustomerLegalSummaryDataOutputDto>(_title, header, data);
        }
    }
}
