namespace Aban360.ReportPool.Domain.Constants
{
    public enum ValidationEstateEnum
    {
        InvalidNationalCode=0,
        InvalidPostalCode=1,
        InvalidFirstName=2,
        InvalidSurName=3,
        EmptyCommercialUnit=4,
        EmptyDomesticUnit=5,
        EmptyOtherUnit=6,
        EmptyCommercialArea=7,
        EmptyDomesticArea=8,
        EmptyFieldArea=9,
        EmptyConstructedArea=10,
        InvalidMobileNumber=11,
        InvalidPhoneNumber=12,
        AddressLessThan10Char=13,
        NonDomesticWithoutContractCapacity=14,
        SewageInstallationDateWithoutSiphon=15,
        UsageIdEqual0=16
    }
}
