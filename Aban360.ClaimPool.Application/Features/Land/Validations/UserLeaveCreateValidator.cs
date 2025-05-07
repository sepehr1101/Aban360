using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class UserLeaveCreateValidator : BaseValidator<UserLeaveCreateDto>
    {
        public UserLeaveCreateValidator()
        {
            RuleFor(f => f.RegiatereId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(f => f.RegiatereFullname)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(f => f.RegiatereDatetime)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(f => f.UserId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .Must(u => u != Guid.Empty).WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(f => f.UserFullname)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(f => f.FromDateJalali)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .Length(10).WithMessage(ExceptionLiterals.Equal10);

            RuleFor(f => f.ToDateJalali)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .Length(10).WithMessage(ExceptionLiterals.Equal10);

        }
        public class UserLeaveUpdateValidator : BaseValidator<UserLeaveUpdateDto>
        {
            public UserLeaveUpdateValidator()
            {
                RuleFor(f => f.Id)
                  .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                  .NotNull().WithMessage(ExceptionLiterals.NotNUll);

                RuleFor(f => f.RegiatereId)
                  .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                  .NotNull().WithMessage(ExceptionLiterals.NotNUll);

                RuleFor(f => f.RegiatereFullname)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

                RuleFor(f => f.RegiatereDatetime)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll);

                RuleFor(f => f.UserId)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .Must(u => u != Guid.Empty).WithMessage(ExceptionLiterals.NotNUll);

                RuleFor(f => f.UserFullname)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

                RuleFor(f => f.FromDateJalali)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .Length(10).WithMessage(ExceptionLiterals.Equal10);

                RuleFor(f => f.ToDateJalali)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .Length(10).WithMessage(ExceptionLiterals.Equal10);
            }
        }
        public class UserWorkdayUpdateValidator : BaseValidator<UserWorkdayUpdateDto>
        {
            public UserWorkdayUpdateValidator()
            {
                RuleFor(f => f.Id)
                 .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                 .NotNull().WithMessage(ExceptionLiterals.NotNUll);

                RuleFor(f => f.UserId)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .Must(u => u != Guid.Empty).WithMessage(ExceptionLiterals.NotNUll);

                RuleFor(f => f.UserFullname)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

                RuleFor(f => f.FromReadingNumber)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .MaximumLength(31).WithMessage(ExceptionLiterals.NotMoreThan31);

                RuleFor(f => f.ToReadingNumber)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .MaximumLength(31).WithMessage(ExceptionLiterals.NotMoreThan31);

                RuleFor(f => f.DateJalali)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .Length(10).WithMessage(ExceptionLiterals.Equal10);

                RuleFor(f => f.ZoneId)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll);
            }
        }
        public class UserWorkdayCreateValidator : BaseValidator<UserWorkdayCreateDto>
        {
            public UserWorkdayCreateValidator()
            {
                RuleFor(f => f.UserId)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .Must(u => u != Guid.Empty).WithMessage(ExceptionLiterals.NotNUll);

                RuleFor(f => f.UserFullname)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

                RuleFor(f => f.FromReadingNumber)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .MaximumLength(31).WithMessage(ExceptionLiterals.NotMoreThan31);

                RuleFor(f => f.ToReadingNumber)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .MaximumLength(31).WithMessage(ExceptionLiterals.NotMoreThan31);

                RuleFor(f => f.DateJalali)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                   .Length(10).WithMessage(ExceptionLiterals.Equal10);

                RuleFor(f => f.ZoneId)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll);
            }
        }
        public class WaterResourceCreateValidator : BaseValidator<WaterResourceCreateDto>
        {
            public WaterResourceCreateValidator()
            {
                RuleFor(f => f.Title)
                 .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                 .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                 .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

                RuleFor(f => f.HeadquartersId)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll);
            }
        }
        public class WaterResourceUpdateValidator : BaseValidator<WaterResourceUpdateDto>
        {
            public WaterResourceUpdateValidator()
            {
                RuleFor(f => f.Id)
                 .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                 .NotNull().WithMessage(ExceptionLiterals.NotNUll);

                RuleFor(f => f.Title)
                  .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                  .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                  .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

                RuleFor(f => f.HeadquartersId)
                   .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                   .NotNull().WithMessage(ExceptionLiterals.NotNUll);
            }
        }
    }
}