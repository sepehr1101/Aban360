using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class UnreadHandler : IUnreadHandler
    {
        private readonly IUnreadQueryService _unreadQueryService;
        private readonly IValidator<UnreadInputDto> _validator;
        public UnreadHandler(
            IUnreadQueryService unreadQueryService,
            IValidator<UnreadInputDto> validator)
        {
            _unreadQueryService = unreadQueryService;
            _unreadQueryService.NotNull(nameof(unreadQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto>> Handle(UnreadInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<UnreadHeaderOutputDto, UnreadDataOutputDto> unread = await _unreadQueryService.GetInfo(input);
            return unread;
        }
    }
}
