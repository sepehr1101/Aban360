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
    internal sealed class ServiceLinkRawItemsDetailHandler : IServiceLinkRawItemsDetailHandler
    {
        private readonly IServiceLinkRawItemsDetailQueryService _serviceLinkRawItemsDetailQuery;
        private readonly IValidator<ServiceLinkRawItemsInputDto> _validator;
        public ServiceLinkRawItemsDetailHandler(
            IServiceLinkRawItemsDetailQueryService serviceLinkRawItemsDetailQuery,
            IValidator<ServiceLinkRawItemsInputDto> validator)
        {
            _serviceLinkRawItemsDetailQuery = serviceLinkRawItemsDetailQuery;
            _serviceLinkRawItemsDetailQuery.NotNull(nameof(serviceLinkRawItemsDetailQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawNetItemsDetailDataOutputDto>> Handle(ServiceLinkRawItemsInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var result = await _serviceLinkRawItemsDetailQuery.Get(input);
            return result;
        }
    }
}
