using Aban360.Common.BaseEntities;
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
    internal sealed class ServiceLinkCalculationDetailsHandler : IServiceLinkCalculationDetailsHandler
    {
        private readonly IServiceLinkCalculationDetailsQueryService _calculationDetailsQueryService;
        private readonly IValidator<ServiceLinkCalculationDetailsInputDto> _validator;
        public ServiceLinkCalculationDetailsHandler(
            IServiceLinkCalculationDetailsQueryService calculationDetailsQueryService,
            IValidator<ServiceLinkCalculationDetailsInputDto> validator)
        {
            _calculationDetailsQueryService = calculationDetailsQueryService;
            _calculationDetailsQueryService.NotNull(nameof(calculationDetailsQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ServiceLinkCalculationDetailsHeaderOutputDto, ServiceLinkCalculationDetailsDataOutputDto>> Handle(ServiceLinkCalculationDetailsInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<ServiceLinkCalculationDetailsHeaderOutputDto, ServiceLinkCalculationDetailsDataOutputDto> calculationDetails = await _calculationDetailsQueryService.GetInfo(input);
            return calculationDetails;
        }
    }
}
