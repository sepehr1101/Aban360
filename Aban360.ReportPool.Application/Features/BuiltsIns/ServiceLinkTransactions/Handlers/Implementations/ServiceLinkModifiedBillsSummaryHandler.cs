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
    internal sealed class ServiceLinkModifiedBillsSummaryHandler : IServiceLinkModifiedBillsSummaryHandler
    {
        private readonly IServiceLinkModifiedBillsSummaryQueryService _modifiedBillsQueryService;
        private readonly IValidator<ServiceLinkModifiedBillsInputDto> _validator;
        public ServiceLinkModifiedBillsSummaryHandler(
            IServiceLinkModifiedBillsSummaryQueryService modifiedBillsQueryService,
            IValidator<ServiceLinkModifiedBillsInputDto> validator)
        {
            _modifiedBillsQueryService = modifiedBillsQueryService;
            _modifiedBillsQueryService.NotNull(nameof(modifiedBillsQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ServiceLinkModifiedBillsHeaderOutputDto, ServiceLinkModifiedBillsSummaryDataOutputDto>> Handle(ServiceLinkModifiedBillsInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ServiceLinkModifiedBillsHeaderOutputDto, ServiceLinkModifiedBillsSummaryDataOutputDto> modifiedBills = await _modifiedBillsQueryService.GetInfo(input);
            return modifiedBills;
        }
    }
}
