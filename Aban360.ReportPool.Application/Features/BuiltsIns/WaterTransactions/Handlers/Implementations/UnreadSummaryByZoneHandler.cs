using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class UnreadSummaryByZoneHandler : IUnreadSummaryByZoneHandler
    {
        private readonly IUnreadSummaryByZoneQueryService _unreadSummaryByZoneQueryService;
        private readonly IValidator<UnreadInputDto> _validator;
        public UnreadSummaryByZoneHandler(
            IUnreadSummaryByZoneQueryService unreadSummaryByZoneQueryService,
            IValidator<UnreadInputDto> validator)
        {
            _unreadSummaryByZoneQueryService = unreadSummaryByZoneQueryService;
            _unreadSummaryByZoneQueryService.NotNull(nameof(unreadSummaryByZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryByZoneDataOutputDto>> Handle(UnreadInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryByZoneDataOutputDto> unreadSummaryByZone = await _unreadSummaryByZoneQueryService.GetInfo(input);
            return unreadSummaryByZone;
        }
    }
}
