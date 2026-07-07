using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Implementations
{
    internal sealed class ConnectDisconnectVeryDetailHandler : IConnectDisconnectVeryDetailHandler
    {
        private readonly IConnectDisconnectQueryService _connectDisconnectQueryService;
        private readonly IValidator<ConnectDisconnectVeryDetailInputDto> _validator;
        private string _title = ReportLiterals.ConnectDisconnectVeryDetail;
        public ConnectDisconnectVeryDetailHandler(
            IConnectDisconnectQueryService connectDisconnectQueryService,
            IValidator<ConnectDisconnectVeryDetailInputDto> validator)
        {
            _connectDisconnectQueryService = connectDisconnectQueryService;
            _connectDisconnectQueryService.NotNull(nameof(connectDisconnectQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ConnectDisconnectVeryDetailHeaderOutputDto, ConnectDisconnectVeryDetailDataOutputDto>> Handle(ConnectDisconnectVeryDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            IEnumerable<ConnectDisconnectVeryDetailDataOutputDto> data = await _connectDisconnectQueryService.Get(inputDto);
            ConnectDisconnectVeryDetailHeaderOutputDto header = new()
            {
                FromDateJalali = inputDto.FromDateJalali,
                ToDateJalali = inputDto.ToDateJalali,
                Title = _title,
                RecordCount = data?.Count() ?? 0,
                CustomerCount = data?.Count() ?? 0,
            };
            return new ReportOutput<ConnectDisconnectVeryDetailHeaderOutputDto, ConnectDisconnectVeryDetailDataOutputDto>(_title, header, data);
        }
    }
}
