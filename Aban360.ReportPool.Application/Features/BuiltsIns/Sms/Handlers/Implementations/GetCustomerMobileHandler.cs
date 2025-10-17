using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.Sms.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.Sms.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.Sms.Handlers.Implementations
{
    internal sealed class GetCustomerMobileHandler : IGetCustomerMobileHandler
    {
        private readonly IGetCustomerMobileQueryService _getCustomerMobileQueryService;
        private readonly IValidator<GetCustomerMobileInputDto> _validator;
        public GetCustomerMobileHandler(
            IGetCustomerMobileQueryService getCustomerMobileQueryService,
            IValidator<GetCustomerMobileInputDto> validator)
        {
            _getCustomerMobileQueryService = getCustomerMobileQueryService;
            _getCustomerMobileQueryService.NotNull(nameof(getCustomerMobileQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<GetCustomerMobileOutputDto> Handle(GetCustomerMobileInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            GetCustomerMobileOutputDto getCustomerMobile = await _getCustomerMobileQueryService.Get(input);
            return getCustomerMobile;
        }
    }
}
