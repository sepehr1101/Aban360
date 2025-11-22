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
            if (!string.IsNullOrWhiteSpace(customerInputDto.FirstName) ||
                !string.IsNullOrWhiteSpace(customerInputDto.Surname) || 
                !string.IsNullOrWhiteSpace(customerInputDto.FatherName) ||
                !string.IsNullOrWhiteSpace(customerInputDto.BillId) ||
                !string.IsNullOrWhiteSpace(customerInputDto.MobileNumber) ||
                !string.IsNullOrWhiteSpace(customerInputDto.Address) ||
                !string.IsNullOrWhiteSpace(customerInputDto.FromReadingNumber) ||
                !string.IsNullOrWhiteSpace(customerInputDto.ToReadingNumber) ||
                !string.IsNullOrWhiteSpace(customerInputDto.PhoneNumber) ||
                !string.IsNullOrWhiteSpace(customerInputDto.PostalCode) ||
                !string.IsNullOrWhiteSpace(customerInputDto.NationalCode) ||
                (customerInputDto.MeterDiameter.HasValue&& customerInputDto.MeterDiameter >= 0) ||
                (customerInputDto.CustomerNumber.HasValue&& customerInputDto.CustomerNumber >= 0) ||
                (customerInputDto.FromContractualCapacity.HasValue && customerInputDto.FromContractualCapacity >= 0) ||
                (customerInputDto.ToContractualCapacity.HasValue && customerInputDto.ToContractualCapacity >= 0) ||
                (customerInputDto.FromHousholderNumber.HasValue && customerInputDto.FromHousholderNumber >= 0) ||
                (customerInputDto.ToHousholderNumber.HasValue && customerInputDto.ToHousholderNumber >= 0) ||
                (customerInputDto.FromUnitDomesticWater.HasValue && customerInputDto.FromUnitDomesticWater >= 0) ||
                (customerInputDto.ToUnitDomesticWater.HasValue && customerInputDto.ToUnitDomesticWater >= 0) ||
                (customerInputDto.FromUnitOtherWater.HasValue && customerInputDto.FromUnitOtherWater >= 0) ||  
                (customerInputDto.FromUnitCommercialWater.HasValue && customerInputDto.FromUnitCommercialWater >= 0) ||
                (customerInputDto.ToUnitCommercialWater.HasValue && customerInputDto.ToUnitCommercialWater >= 0) ||              
                (customerInputDto.ToUnitOtherWater.HasValue && customerInputDto.ToUnitOtherWater >= 0) ||
                 customerInputDto.ZoneIds.Count>0||
                 customerInputDto.UsageIds.Count>0)
            {
                return true;
            }

            return false;
        }
    }
}
