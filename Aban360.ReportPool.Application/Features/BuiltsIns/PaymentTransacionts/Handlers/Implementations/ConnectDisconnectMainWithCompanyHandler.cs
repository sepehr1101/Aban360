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
    internal sealed class ConnectDisconnectMainWithCompanyHandler : IConnectDisconnectMainWithCompanyHandler
    {
        private readonly IConnectDisconnectQueryService _connectDisconnectQueryService;
        private readonly IValidator<ConnectDisconnectMainInputDto> _validator;
        private string _title = ReportLiterals.ConnectDisconnectMain;
        public ConnectDisconnectMainWithCompanyHandler(
            IConnectDisconnectQueryService connectDisconnectQueryService,
            IValidator<ConnectDisconnectMainInputDto> validator)
        {
            _connectDisconnectQueryService = connectDisconnectQueryService;
            _connectDisconnectQueryService.NotNull(nameof(connectDisconnectQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ConnectDisconnectMainHeaderOutputDto, ConnectDisconnectMainByCompanyDataOutputDto>> Handle(ConnectDisconnectMainInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            IEnumerable<ConnectDisconnectMainByCompanyDataOutputDto> data = await _connectDisconnectQueryService.GetWithCompany(inputDto);
            ConnectDisconnectMainHeaderOutputDto header = new()
            {
                ZoneCount = data?.DistinctBy(d => d.ZoneTitle)?.Count() ?? 0,
                FromDateJalali = inputDto.FromDateJalali,
                ToDateJalali = inputDto.ToDateJalali,
                Title = _title,
                RecordCount = data?.Count() ?? 0,
                CustomerCount = data?.Sum(d => d.Count) ?? 0,
            };
            return new ReportOutput<ConnectDisconnectMainHeaderOutputDto, ConnectDisconnectMainByCompanyDataOutputDto>(_title, header, data);
        }
    }
}
