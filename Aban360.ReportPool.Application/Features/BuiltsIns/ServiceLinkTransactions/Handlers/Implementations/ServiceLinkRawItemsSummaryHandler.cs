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
    internal sealed class ServiceLinkRawItemsSummaryHandler : IServiceLinkRawItemsSummaryHandler
    {
        private readonly IServiceLinkRawItemsSummaryQueryService _serviceLinkRawItemsSummaryQuery;
        private readonly IValidator<ServiceLinkRawItemsInputDto> _validator;
        public ServiceLinkRawItemsSummaryHandler(
            IServiceLinkRawItemsSummaryQueryService serviceLinkRawItemsSummaryQuery,
            IValidator<ServiceLinkRawItemsInputDto> validator)
        {
            _serviceLinkRawItemsSummaryQuery = serviceLinkRawItemsSummaryQuery;
            _serviceLinkRawItemsSummaryQuery.NotNull(nameof(serviceLinkRawItemsSummaryQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawNetItemsSummaryDataOutputDto>> Handle(ServiceLinkRawItemsInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var result = await _serviceLinkRawItemsSummaryQuery.Get(input);
            return result;
        }
    }
}
