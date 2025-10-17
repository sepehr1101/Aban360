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
    internal sealed class ServiceLinkNetItemsSummaryHandler : IServiceLinkNetItemsSummaryHandler
    {
        private readonly IServiceLinkNetItemsSummaryQueryService _serviceLinkNetItemsSummaryQuery;
        private readonly IValidator<ServiceLinkNetItemsInputDto> _validator;
        public ServiceLinkNetItemsSummaryHandler(
            IServiceLinkNetItemsSummaryQueryService serviceLinkNetItemsSummaryQuery,
            IValidator<ServiceLinkNetItemsInputDto> validator)
        {
            _serviceLinkNetItemsSummaryQuery = serviceLinkNetItemsSummaryQuery;
            _serviceLinkNetItemsSummaryQuery.NotNull(nameof(serviceLinkNetItemsSummaryQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkRawNetItemsSummaryDataOutputDto>> Handle(ServiceLinkNetItemsInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var result = await _serviceLinkNetItemsSummaryQuery.Get(input);
            return result;
        }
    }
}
