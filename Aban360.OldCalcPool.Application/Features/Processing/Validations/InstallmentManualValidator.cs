using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Processing.Validations
{
    public class InstallmentManualValidator : BaseValidator<BillInstallmentManualInputDto>
    {
        public InstallmentManualValidator()
        {
            RuleFor(g => g.BillId)
            .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(g => g.Installments)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .Must(CheckDueDateJalali).WithMessage(ExceptionLiterals.InvalidLessThanCurrentDate);
        }
        private bool CheckDueDateJalali(ICollection<InstallmentDataInputDto> Installments)
        {
            string currentDateJalali = DateTime.Now.ToShortPersianDateString();
            foreach (var installment in Installments)
            {
                if (installment.DeadLineDateJalali.CompareTo(currentDateJalali) < 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
