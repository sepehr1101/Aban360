using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class HandoverDetailHandler : IHandoverDetailHandler
    {
        private readonly IHandoverDetailQueryService _handoverDetailQueryService;
        private readonly IValidator<HandoverInputDto> _validator;
        public HandoverDetailHandler(
            IHandoverDetailQueryService handoverDetailQueryService,
            IValidator<HandoverInputDto> validator)
        {
            _handoverDetailQueryService = handoverDetailQueryService;
            _handoverDetailQueryService.NotNull(nameof(handoverDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<HandoverHeaderOutputDto, HandoverDetailDataOutputDto>> Handle(HandoverInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<HandoverHeaderOutputDto, HandoverDetailDataOutputDto> HandoverDetail = await _handoverDetailQueryService.Get(input);
            return HandoverDetail;
        }
    }
}
