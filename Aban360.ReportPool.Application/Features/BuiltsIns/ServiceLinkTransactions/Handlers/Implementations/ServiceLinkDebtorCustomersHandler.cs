using Aban360.Common.Excel;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class ServiceLinkDebtorCustomersHandler : IServiceLinkDebtorCustomersHandler
    {
        private readonly IServiceLinkDebtorCustomersQueryService _serviceLinkDebtorCustomersQueryService;
        private readonly IValidator<ServiceLinkDebtorCustomersInputDto> _validator;
        public ServiceLinkDebtorCustomersHandler(
            IServiceLinkDebtorCustomersQueryService ServiceLinkDebtorCustomersQueryService,
            IValidator<ServiceLinkDebtorCustomersInputDto> validator)
        {
            _serviceLinkDebtorCustomersQueryService = ServiceLinkDebtorCustomersQueryService;
            _serviceLinkDebtorCustomersQueryService.NotNull(nameof(ServiceLinkDebtorCustomersQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ServiceLinkDebtorCustomersHeaderOutputDto, ServiceLinkDebtorCustomersDataOutputDto>> Handle(ServiceLinkDebtorCustomersInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ServiceLinkDebtorCustomersHeaderOutputDto, ServiceLinkDebtorCustomersDataOutputDto> ServiceLinkDebtorCustomers = await _serviceLinkDebtorCustomersQueryService.GetInfo(input);
            return ServiceLinkDebtorCustomers;
        }
    }
}
