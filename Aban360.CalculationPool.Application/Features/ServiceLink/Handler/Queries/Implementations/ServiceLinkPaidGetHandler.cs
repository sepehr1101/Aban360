using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Implementations
{
    internal sealed class ServiceLinkPaidGetHandler : IServiceLinkPaidGetHandler
    {
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneQueryServcice;
        private readonly IVosolEnQueryService _vosolEnQueryService;
        private readonly IVariabService _variabService;
        private string _title = ReportLiterals.ServiceLinkPaid;
        string _todayJalali = DateTime.Now.ToShortPersianDateString();
        string _30DayAgoDateJalali = DateTime.Now.AddDays(-30).ToShortPersianDateString();
        public ServiceLinkPaidGetHandler(
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneQueryService,
            IVosolEnQueryService vosolEnQueryService,
            IVariabService variabService)
        {
            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneQueryServcice = commonZoneQueryService;
            _commonZoneQueryServcice.NotNull(nameof(commonZoneQueryService));

            _vosolEnQueryService = vosolEnQueryService;
            _vosolEnQueryService.NotNull(nameof(vosolEnQueryService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));
        }

        public async Task<ReportOutput<ServiceLinkPaidHeaderOutputDto, ServiceLinkPaidDataOutputDto>> Handle(ServiceLinkPaidInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _commonZoneQueryServcice.IsUserInZone(appUser, input.ZoneId);
            IEnumerable<ServiceLinkPaidDataOutputDto> data = await _vosolEnQueryService.Get(input);
            await ValidateDates(data);
            ServiceLinkPaidHeaderOutputDto header = new()
            {
                ZoneId = input.ZoneId,
                ZoneTitle = data?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                Amount = data?.Sum(a => a.Amount) ?? 0,
                RecordCount = data?.Count() ?? 0,
                Title = _title
            };

            return new ReportOutput<ServiceLinkPaidHeaderOutputDto, ServiceLinkPaidDataOutputDto>(_title, header, data);
        }
        private async Task ValidateDates(IEnumerable<ServiceLinkPaidDataOutputDto> input)
        {
            foreach (var item in input)
            {
                string checkDateJalali = await _variabService.GetDateCheck(item.ZoneId);
                DateOnly? dateOnlyBank = item.BankDateJalali.ToGregorianDateOnly();
                DateOnly? dateOnlyPay = item.PayDateJalali.ToGregorianDateOnly();

                if (!dateOnlyBank.HasValue || !dateOnlyPay.HasValue)
                {
                    throw new InvalidDateException(ExceptionLiterals.InvalidDate);
                }
                if (_todayJalali.CompareTo(checkDateJalali) < 0)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidPaymentInsertAfterDateCheck);
                }

                if (item.PayDateJalali.CompareTo(_todayJalali) > 0)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidMoreThanCurrentDate);
                }
                if (item.PayDateJalali.CompareTo(checkDateJalali) <= 0)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidPaymentInsertAfterDateCheck);
                }

                if (item.BankDateJalali.CompareTo(_todayJalali) > 0)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidMoreThanCurrentDate);
                }
                if (item.BankDateJalali.CompareTo(checkDateJalali) <= 0)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidPaymentInsertAfterDateCheck);
                }
                if (item.BankDateJalali.CompareTo(_30DayAgoDateJalali) < 0)
                {
                    throw new InvalidBillCommandException(ExceptionLiterals.InvalidBankDateBefor30DaysAgo);
                }
            }
        }
    }
}
