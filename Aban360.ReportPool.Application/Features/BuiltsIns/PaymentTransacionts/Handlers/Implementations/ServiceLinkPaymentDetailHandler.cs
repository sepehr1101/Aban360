using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class ServiceLinkPaymentDetailHandler : IServiceLinkPaymentDetailHandler
    {
        private readonly IServiceLinkPaymentDetailQueryService _serviceLinkPaymentDetailQueryService;
        private readonly IValidator<ServiceLinkPaymentDetailInputDto> _validator;
        public ServiceLinkPaymentDetailHandler(
            IServiceLinkPaymentDetailQueryService serviceLinkPaymentDetailQueryService,
            IValidator<ServiceLinkPaymentDetailInputDto> validator)
        {
            _serviceLinkPaymentDetailQueryService = serviceLinkPaymentDetailQueryService;
            _serviceLinkPaymentDetailQueryService.NotNull(nameof(serviceLinkPaymentDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ServiceLinkPaymentDetailHeaderOutputDto, ServiceLinkPaymentDetailDataOutputDto>> Handle(ServiceLinkPaymentDetailInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ServiceLinkPaymentDetailHeaderOutputDto, ServiceLinkPaymentDetailDataOutputDto> serviceLinkPaymentDetail = await _serviceLinkPaymentDetailQueryService.GetInfo(input);
            return serviceLinkPaymentDetail;
        }
    }
}
