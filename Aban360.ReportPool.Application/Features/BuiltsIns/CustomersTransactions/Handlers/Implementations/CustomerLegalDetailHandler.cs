using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class CustomerLegalDetailHandler : ICustomerLegalDetailHandler
    {
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        private string _title = ReportLiterals.CustomerLegalDetail;
        public CustomerLegalDetailHandler(ICustomerInfoQueryService customerInfoQueryService)
        {
            _customerInfoQueryService = customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));
        }

        public async Task<ReportOutput<CustomerLegalDetailHeaderOutputDto, CustomerLegalDetailDataOutputDto>> Handle(CustomerLegalInputDto input, CancellationToken cancellationToken)
        {
            IEnumerable<CustomerLegalDetailDataOutputDto> data = await _customerInfoQueryService.GetDetail(input);
            CustomerLegalDetailHeaderOutputDto header = new()
            {
                ZoneCount = input?.ZoneIds?.Count() ?? 0,
                CustomerCount = data?.Count() ?? 0,
                RecordCount = data?.Count() ?? 0,
                Title =_title   ,
            };
            return new ReportOutput<CustomerLegalDetailHeaderOutputDto, CustomerLegalDetailDataOutputDto>(_title, header, data);
        }
    }
}
