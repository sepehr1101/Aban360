using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Validations
{
    public class CustomerSearchAdvancedValidator : BaseValidator<CustomerSearchAdvancedInputDto>
    {
        public CustomerSearchAdvancedValidator()
        {

            RuleFor(customer => customer)
            .Must(CustomerSearchAdvancedValidations).WithMessage(ExceptionLiterals.NotNullAll);
        }

        private bool CustomerSearchAdvancedValidations(CustomerSearchAdvancedInputDto customerInputDto)
        {
            if (!string.IsNullOrWhiteSpace(customerInputDto.ReadingNumber) ||
                !string.IsNullOrWhiteSpace(customerInputDto.FirstName) ||
                !string.IsNullOrWhiteSpace(customerInputDto.Surname) ||
                !string.IsNullOrWhiteSpace(customerInputDto.BillId) ||
                !string.IsNullOrWhiteSpace(customerInputDto.MobileNumber) ||
                !string.IsNullOrWhiteSpace(customerInputDto.Address) ||
                (customerInputDto.CustomerNumber.HasValue&& customerInputDto.CustomerNumber > 0) ||
                (customerInputDto.FromUnitDomesticWater.HasValue && customerInputDto.FromUnitDomesticWater > 0) ||
                (customerInputDto.ToUnitDomesticWater.HasValue && customerInputDto.ToUnitDomesticWater > 0) ||
                (customerInputDto.FromUnitDomesticWater.HasValue && customerInputDto.FromUnitDomesticWater > 0) ||
                (customerInputDto.ToUnitDomesticWater.HasValue && customerInputDto.ToUnitDomesticWater > 0) ||
                (customerInputDto.FromUnitOtherWater.HasValue && customerInputDto.FromUnitOtherWater > 0) ||
                (customerInputDto.ToUnitOtherWater.HasValue && customerInputDto.ToUnitOtherWater > 0) )
            {
                return true;
            }


            return false;
        }
    }
}
