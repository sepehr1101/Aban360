using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts
{
    internal sealed class MeterReadingDetailUpdateHandler : IMeterReadingDetailUpdateHandler
    {
        private readonly IMeterReadingDetailService _meterReadingDetailService;
        private readonly IValidator<MeterReadingDetailUpdateDto> _validator;
        public MeterReadingDetailUpdateHandler(
             IMeterReadingDetailService meterReadingDetailService,
             IValidator<MeterReadingDetailUpdateDto> validator)
        {
            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }

        public async Task Handle(MeterReadingDetailUpdateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            //insert
            MeterReadingDetailCreateDuplicateDto readingCreateDuplicate = new(input.Id, input.CurrentCounterStateCode, input.CurrentDateJalali, input.CurrentNumber, appUser.UserId, DateTime.Now);
            await _meterReadingDetailService.CreateDuplicateForLog(readingCreateDuplicate);

            //removed previous
            MeterReadingDetailDeleteDto readingDelete = new(input.Id, appUser.UserId, DateTime.Now);
            await _meterReadingDetailService.Delete(readingDelete);

        }
    }
}