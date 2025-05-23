using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal sealed class CustomerMobileInfoListHandler : ICustomerMobileInfoListHandler
    {
        private readonly ICustomerMobileListService _customerMobileListService;
        public CustomerMobileInfoListHandler(ICustomerMobileListService customerMobileListService)
        {
            _customerMobileListService = customerMobileListService;
            _customerMobileListService.NotNull(nameof(customerMobileListService));
        }

        public async Task<IEnumerable<BillIdMobileDto>> Handle(BillIdListDtoWrapper billIdListDtoWrapper, CancellationToken cancellationToken)
        {
            IEnumerable<BillIdMobileDto> billIdMobileDtos = await _customerMobileListService.GetCustomerMobileInfo(billIdListDtoWrapper);
            return billIdMobileDtos;
        }
    }
}
