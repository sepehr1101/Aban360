using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    //internal class BillInstallmentHandler : IBillInstallmentHandler
    //{
    //    private readonly IValidator<InstallmentInputDto> _validator;
    //    public BillInstallmentHandler(IValidator<InstallmentInputDto> validator)
    //    {
    //        _validator = validator;
    //        _validator.NotNull(nameof(validator));
    //    }

    //    public async Task<ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto>> Handle(InstallmentInputDto inputDto, CancellationToken cancellationToken)
    //    {
    //        var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
    //        if (!validationResult.IsValid)
    //        {
    //            var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
    //            throw new CustomValidationException(message);
    //        }

    //        string title = "اقساط آب‌بها";
    //        double amount = 53000000;
    //        double firstInstallmentAmount = 0;
    //        ICollection<InstallmentDataOutputDto> data = new List<InstallmentDataOutputDto>();
    //        for (int i = 0; i < inputDto.InstallmentCount; i++)
    //        {
    //            firstInstallmentAmount = amount * inputDto.AdvancePaymentPercentage / 100;

    //            double localAmount = i == 0 ?
    //                localAmount = firstInstallmentAmount :
    //                localAmount = (amount - firstInstallmentAmount) / ((inputDto.InstallmentCount - 1));

    //            InstallmentDataOutputDto item = new()
    //            {
    //                Amount = localAmount,
    //                BillId = inputDto.BillId,
    //                DueDateJalali = DateTime.Now.AddMonths(i).ToShortPersianDateString(),
    //                InstallmentOrder = i + 1,
    //                PaymentId = inputDto.BillId + "361" + i.ToString()
    //            };
    //            data.Add(item);
    //        }

    //        InstallmentHeaderOutputDto header = new InstallmentHeaderOutputDto()
    //        {
    //            BillId = inputDto.BillId,
    //            AdvancePaymentPercentage = inputDto.AdvancePaymentPercentage,
    //            Amount = data.Sum(x => x.Amount),
    //            InstallmentCount = data.Count(),
    //            TrackNumber = inputDto.TrackNumber,
    //        };
    //        ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto> result = new(title, header, data);

    //        return result;
    //    }
    //}
}
