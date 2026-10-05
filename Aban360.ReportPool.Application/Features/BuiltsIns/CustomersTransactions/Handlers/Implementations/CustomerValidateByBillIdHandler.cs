using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class CustomerValidateByBillIdHandler : ICustomerValidateByBillIdHandler
    {
        private readonly ICommonMemberQueryService _customerInfoQueryService;
        private readonly ICommonZoneService _zoneService;
        private readonly IValidator<CustomerInfoByZoneAndCustomerNumberInputDto> _validator;
        public CustomerValidateByBillIdHandler(
            ICommonMemberQueryService customerInfoQueryService,
            ICommonZoneService zoneService,
            IValidator<CustomerInfoByZoneAndCustomerNumberInputDto> validator)
        {
            _customerInfoQueryService = customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<CustomerBillIdValidateDto> Handle(SearchInput input, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber customerInfo = await _customerInfoQueryService.Get(input.Input, false);

            bool isValidBillId;
            if (customerInfo is not null)
            {
                isValidBillId = true;
                await _zoneService.IsUserInZone(appUser, customerInfo.ZoneId);
            }
            else
            {
                isValidBillId = false;
            }
            return new CustomerBillIdValidateDto(input.Input, isValidBillId);
        }
    }
}
