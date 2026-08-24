using Aban360.MeterPool.Domain.Features.Reading;

namespace Aban360.MeterPool.Application.Features.Reading
{
    internal sealed class Load : ILoad
    {
        public Task<IReadOnlyCollection<ReadingLoadDto>> Handle(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IReadOnlyCollection<ReadingLoadDto> readings =
            [
                new()
                {
                    Id = 1,
                    UsageTitle = "مسکونی",
                    UsageId = 1,
                    BranchTypeId = 3,
                    BranchTypeTitle = "آب و فاضلاب",
                    PreviousNumber = 1248,
                    PreviousDateJalalai = "1405/04/15",
                    PreviousCounterStateId = 1,
                    PreviousCounterStateTitle = "عادی",
                    Firstname = "محمد",
                    Surename = "احمدی",
                    Address = "تهران، خیابان آزادی، کوچه یاس، پلاک ۱۲",
                    BillId = "1012345678901",
                    CustomerNumber = 120001,
                    ReadingNumber = "010120001",
                    MobileNumber = "09121234567",
                    TrackNumber = 500001,
                    Debt = 1_250_000,
                    PostalCode = "1345678910",
                    MainSiphonTitle = "سیفون ۱۰۰",
                    MainSiphonId = 100,
                    X = "51.3498",
                    Y = "35.7002",
                    DomesticUnit = 2,
                    CommercialUnit = 0,
                    OtherUnit = 0,
                    DiscountId = 0,
                    DiscountTitle = "بدون تخفیف",
                    WaterDiameterId = 15,
                    WaterDiameterTitle = "۱۵ میلی‌متر",
                    WaterInstallationDateJalali = "1398/08/12",
                    SewageInstallationDateJalali = "1400/03/20",
                    GuildId = 0,
                    GuildTitle = "فاقد صنف",
                    HouseholdUnit = 6,
                    HouseholdDateJalali = "1404/01/01"
                },
                new()
                {
                    Id = 2,
                    UsageTitle = "تجاری",
                    UsageId = 2,
                    BranchTypeId = 1,
                    BranchTypeTitle = "آب",
                    PreviousNumber = 8935,
                    PreviousDateJalalai = "1405/04/16",
                    PreviousCounterStateId = 1,
                    PreviousCounterStateTitle = "عادی",
                    Firstname = "مریم",
                    Surename = "کریمی",
                    Address = "تهران، خیابان ولیعصر، مجتمع تجاری نور، واحد ۸",
                    BillId = "1012345678902",
                    CustomerNumber = 120002,
                    ReadingNumber = "010120002",
                    MobileNumber = "09123334455",
                    TrackNumber = 500002,
                    Debt = 4_870_000,
                    PostalCode = "1435678921",
                    MainSiphonTitle = "فاقد سیفون",
                    MainSiphonId = 0,
                    X = "51.4071",
                    Y = "35.7219",
                    DomesticUnit = 0,
                    CommercialUnit = 1,
                    OtherUnit = 0,
                    DiscountId = 0,
                    DiscountTitle = "بدون تخفیف",
                    WaterDiameterId = 20,
                    WaterDiameterTitle = "۲۰ میلی‌متر",
                    WaterInstallationDateJalali = "1395/11/05",
                    SewageInstallationDateJalali = "",
                    GuildId = 12,
                    GuildTitle = "رستوران و تهیه غذا",
                    HouseholdUnit = 0,
                    HouseholdDateJalali = ""
                },
                new()
                {
                    Id = 3,
                    UsageTitle = "مختلط",
                    UsageId = 3,
                    BranchTypeId = 3,
                    BranchTypeTitle = "آب و فاضلاب",
                    PreviousNumber = 3270,
                    PreviousDateJalalai = "1405/04/17",
                    PreviousCounterStateId = 4,
                    PreviousCounterStateTitle = "کنتور تعویض شده",
                    Firstname = "علی",
                    Surename = "رضایی",
                    Address = "تهران، خیابان شریعتی، کوچه بهار، پلاک ۲۷",
                    BillId = "1012345678903",
                    CustomerNumber = 120003,
                    ReadingNumber = "010120003",
                    MobileNumber = "09351234567",
                    TrackNumber = 500003,
                    Debt = 760_000,
                    PostalCode = "1665678932",
                    MainSiphonTitle = "سیفون ۱۲۵",
                    MainSiphonId = 125,
                    X = "51.4384",
                    Y = "35.7448",
                    DomesticUnit = 3,
                    CommercialUnit = 1,
                    OtherUnit = 1,
                    DiscountId = 2,
                    DiscountTitle = "خانواده تحت پوشش",
                    WaterDiameterId = 25,
                    WaterDiameterTitle = "۲۵ میلی‌متر",
                    WaterInstallationDateJalali = "1401/02/18",
                    SewageInstallationDateJalali = "1401/06/09",
                    GuildId = 7,
                    GuildTitle = "خواربارفروشی",
                    HouseholdUnit = 9,
                    HouseholdDateJalali = "1403/07/01"
                }
            ];

            return Task.FromResult(readings);
        }
    }
}
