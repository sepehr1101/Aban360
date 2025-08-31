using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.Sms.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.Sms.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.Sms.Handlers.Implementations
{
    internal sealed class SendSmsToMobileHandler : ISendSmsToMobileHandler
    {
        private readonly ISendSmsToMobileQueryService _sendSmsToMobileQueryService;
        private readonly IValidator<SendSmsToMobileInputDto> _validator;
        public SendSmsToMobileHandler(
            ISendSmsToMobileQueryService sendSmsToMobileQueryService,
            IValidator<SendSmsToMobileInputDto> validator)
        {
            _sendSmsToMobileQueryService = sendSmsToMobileQueryService;
            _sendSmsToMobileQueryService.NotNull(nameof(sendSmsToMobileQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SendSmsToMobileHeaderOutputDto, SendSmsToMobileDataOutputDto>> Handle(SendSmsToMobileInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<SendSmsToMobileHeaderOutputDto, SendSmsToMobileDataOutputDto> sendSmsToMobile = await _sendSmsToMobileQueryService.Get(input);
            return sendSmsToMobile;
        }
    }
}
