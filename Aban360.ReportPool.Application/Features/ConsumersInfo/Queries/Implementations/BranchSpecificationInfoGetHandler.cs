using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using DNTPersianUtils.Core;
using System.Data;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class BranchSpecificationInfoGetHandler : IBranchSpecificationInfoGetHandler
    {
        private readonly IBranchSpecificationInfoService _BranchSpecificationSummaryInfoService;
        public BranchSpecificationInfoGetHandler(IBranchSpecificationInfoService BranchSpecificationSummaryInfoService)
        {
            _BranchSpecificationSummaryInfoService = BranchSpecificationSummaryInfoService;
            _BranchSpecificationSummaryInfoService.NotNull(nameof(BranchSpecificationSummaryInfoService));
        }

        public async Task<BranchSpecificationInfoDto> Handle(string billId, CancellationToken cancellationToken)
        {
            BranchSpecificationInfoDto result = await _BranchSpecificationSummaryInfoService.GetInfo(billId);

            result.MeterLife = CalcLife(result.LatestMeterChangeDate is null ?
                                result.WaterInstallDate : result.LatestMeterChangeDate);

            result.SiphonLife = CalcLife(result.SiphonInstallationDate);

            return result;
        }
        private string CalcLife(string date)
        {
            DateOnly? persianDate = date.ToGregorianDateOnly();
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            if (!persianDate.HasValue)
                return ExceptionLiterals.InvalidDate;


            int totalLifeInDay = (currentDate.DayNumber - persianDate.Value.DayNumber);
            int years = totalLifeInDay / (int)365;
            int remainedYear = totalLifeInDay % 365;
            int months = remainedYear / 30;
            int days = remainedYear % 30;
            return $"{years} سال - {months} ماه - {days} روز";

        }
    }
}
