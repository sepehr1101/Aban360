namespace Aban360.ReportPool.Domain.Constants
{
    public enum ValidationEstateEnum
    {
        InvalidNationalCode = 0,
        InvalidPostalCode = 1,
        InvalidFirstName = 2,
        InvalidSurName = 3,
        EmptyUnits = 4,
        EmptyAreas = 5,
        EmptyConstructedArea = 6,
        InvalidMobileNumber = 7,
        InvalidPhoneNumber = 8,
        AddressLessThan10Char = 9,
        NonDomesticWithoutContractCapacity = 10,
        SewageInstallationDateWithoutSiphon = 11,
        UsageIdEqual0 = 12,
        InvalidHousholdDate= 13,
        InvalidEmptyUnit=14
    }
}
