using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class CustomerInfoWithSameMobileNumberHandler : ICustomerInfoWithSameMobileNumberHandler
    {
        private readonly ICustomerInfoWithSameMobileNumberQueryService _customerInfoWithSameMobileNumberQueryService;
        public CustomerInfoWithSameMobileNumberHandler(
            ICustomerInfoWithSameMobileNumberQueryService customerInfoWithSameMobileNumberQueryService)
        {
            _customerInfoWithSameMobileNumberQueryService = customerInfoWithSameMobileNumberQueryService;
            _customerInfoWithSameMobileNumberQueryService.NotNull(nameof(customerInfoWithSameMobileNumberQueryService));
        }

        public async Task<ReportOutput<CustomerInfoWithSameMobileNumberHeaderOutputDto, CustomerInfoWithSameMobileNumberDataOutputDto>> Handle(string mobileNumber, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerInfoWithSameMobileNumberHeaderOutputDto, CustomerInfoWithSameMobileNumberDataOutputDto> CustomerInfoWithSameMobileNumber = await _customerInfoWithSameMobileNumberQueryService.GetInfo(mobileNumber);
            return CustomerInfoWithSameMobileNumber;
        }
    }
}
