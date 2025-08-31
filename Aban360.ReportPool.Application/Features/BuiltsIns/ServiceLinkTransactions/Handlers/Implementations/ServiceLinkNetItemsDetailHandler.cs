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
    internal sealed class ServiceLinkNetItemsDetailHandler : IServiceLinkNetItemsDetailHandler
    {
        private readonly IServiceLinkNetItemsDetailQueryService _serviceLinkNetItemsDetailQuery;
        private readonly IValidator<ServiceLinkNetItemsInputDto> _validator;
        public ServiceLinkNetItemsDetailHandler(
            IServiceLinkNetItemsDetailQueryService serviceLinkNetItemsDetailQuery,
            IValidator<ServiceLinkNetItemsInputDto> validator)
        {
            _serviceLinkNetItemsDetailQuery = serviceLinkNetItemsDetailQuery;
            _serviceLinkNetItemsDetailQuery.NotNull(nameof(serviceLinkNetItemsDetailQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkNetItemsDetailDataOutputDto>> Handle(ServiceLinkNetItemsInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var result = await _serviceLinkNetItemsDetailQuery.Get(input);
            return result;
        }
    }
}
