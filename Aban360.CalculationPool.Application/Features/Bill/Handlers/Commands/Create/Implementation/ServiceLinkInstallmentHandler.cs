using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal class ServiceLinkInstallmentHandler : IServiceLinkInstallmentHandler
    {
        private readonly IValidator<ServiceLinkInstallmentInputDto> _validator;
        public ServiceLinkInstallmentHandler(IValidator<ServiceLinkInstallmentInputDto> validator)
        {
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto>> Handle(ServiceLinkInstallmentInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            string title = "اقساط حق انشعاب";
            double amount = 160000000;
            double firstInstallmentAmount = 0;
            string[] dueDatelist = GetDate(inputDto.InstallmentCount, inputDto.monthlyDuration);
            ICollection<InstallmentDataOutputDto> data = new List<InstallmentDataOutputDto>();
            for (int i = 0; i < inputDto.InstallmentCount; i++)
            {
                firstInstallmentAmount = amount * inputDto.AdvancePaymentPercentage / 100;

                double localAmount = i == 0 ?
                    localAmount = firstInstallmentAmount :
                    localAmount = (amount - firstInstallmentAmount) / ((inputDto.InstallmentCount - 1));

                InstallmentDataOutputDto item = new()
                {
                    Amount = localAmount,
                    BillId = inputDto.BillId,
                    DueDateJalali = dueDatelist[i],// DateTime.Now.AddMonths(i).ToShortPersianDateString(),
                    InstallmentOrder = i + 1,
                    PaymentId = inputDto.BillId + "361" + i.ToString()
                };
                data.Add(item);
            }

            InstallmentHeaderOutputDto header = new InstallmentHeaderOutputDto()
            {
                BillId = inputDto.BillId,
                AdvancePaymentPercentage = inputDto.AdvancePaymentPercentage,
                Amount = data.Sum(x => x.Amount),
                InstallmentCount = data.Count(),
                TrackNumber = "-"///inputDto.TrackNumber,
            };
            ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto> result = new(title, header, data);

            return result;
        }
        private string[] GetDate(int installmentCount, int monthlyDuration)
        {
            string dateJalali = DateTime.Now.ToShortPersianDateString();


            string[] dateJalaliItems = dateJalali.Split('/');
            short day = Convert.ToInt16(dateJalaliItems[2]);

            string formalDay = "01";
            if (day > 25 || day <= 5)
                formalDay = "05";
            else if (day > 5 && day <= 15)
                formalDay = "15";
            else if (day > 15 && day <= 25)
                formalDay = "25";
            string firstDateJalali = $@"{dateJalaliItems[0]}/{dateJalaliItems[1]}/{formalDay}";

            string[] dueDatesJalali = new string[installmentCount];
            dueDatesJalali[0] = firstDateJalali;
            int month = Convert.ToInt32(dateJalaliItems[1]);
            for (int i = 1; i < installmentCount; i++)
            {
                month = month + monthlyDuration;
                if (month > 12)
                {
                    month = month - 12;
                    dateJalaliItems[0] = (Convert.ToInt32(dateJalaliItems[0]) + 1).ToString();
                }

                string formalMonth = month.ToString();
                if (formalMonth.Length != 2)
                {
                    formalMonth = $@"0{month.ToString()}";
                }
                string date = $@"{dateJalaliItems[0]}/{formalMonth}/{formalDay}";
                dueDatesJalali[i] = date;
            }
            return dueDatesJalali;
        }
    }
}
