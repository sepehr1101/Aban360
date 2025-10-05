using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class UseStateReportSummaryByUsageHandler : IUseStateReportSummaryByUsageHandler
    {
        private readonly IUseStateReportSummaryByUsageQueryService _userStateReportQueryService;
        private readonly IValidator<UseStateReportInputDto> _validator;
        public UseStateReportSummaryByUsageHandler(
            IUseStateReportSummaryByUsageQueryService userStateReportQueryService,
            IValidator<UseStateReportInputDto> validator)
        {
            _userStateReportQueryService = userStateReportQueryService;
            _userStateReportQueryService.NotNull(nameof(userStateReportQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryDataOutputDto>> Handle(UseStateReportInputDto input, CancellationToken cancellationToken)
        {
            var validationReuslt = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationReuslt.IsValid)
            {
                var message = string.Join(", ", validationReuslt.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }
            ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryDataOutputDto> useStateReports = await _userStateReportQueryService.Get(input);
            useStateReports.ReportHeader.ReportDateJalali = DateTime.Now.FormatDateToShortPersianDate();
            return useStateReports;
        }
    }
}
