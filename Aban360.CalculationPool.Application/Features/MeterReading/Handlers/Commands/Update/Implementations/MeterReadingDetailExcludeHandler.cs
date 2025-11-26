using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Excluded.Implementations
{
    internal sealed class MeterReadingDetailExcludeHandler : IMeterReadingDetailExcludeHandler
    {
        private readonly IMeterReadingDetailService _meterReadingDetailService;
        public MeterReadingDetailExcludeHandler(IMeterReadingDetailService meterReadingDetailService)
        {
            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));
        }

        public async Task Handle(int id, IAppUser appUser, CancellationToken cancellationToken)
        {
            MeterReadingDetailExcludedDto readingCreateExcluded = new(id, appUser.UserId, DateTime.Now);
            await _meterReadingDetailService.Exclude(readingCreateExcluded);
        }
    }
}
