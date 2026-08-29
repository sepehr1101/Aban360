using Aban360.MeterPool.Domain.Features.Reading;

namespace Aban360.MeterPool.Application.Features.Reading
{
    internal sealed class Load : ILoad
    {
        public Task<IReadOnlyCollection<ReadingLoadDto>> Handle(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IReadOnlyCollection<ReadingLoadDto> readings = Enumerable
                .Range(1, 1000)
                .Select(index => new ReadingLoadDto
                {
                    Id = index,
                    UsageTitle = $"مسکونی {index}",
                    UsageId = index,
                    BranchTypeId = index,
                    BranchTypeTitle = $"آب و فاضلاب {index}",
                    PreviousNumber = 1000 + index,
                    PreviousDateJalali = $"1405/04/{((index - 1) % 28) + 1:D2}",
                    PreviousCounterStateId = (short)index,
                    PreviousCounterStateTitle = $"عادی {index}",
                    Firstname = $"علی {index}",
                    Surname = $"رضایی {index}",
                    Address = $"تهران، خیابان آزادی، کوچه یاس، پلاک {index}",
                    BillId = $"101234567{index:D4}",
                    CustomerNumber = 120000 + index,
                    ReadingNumber = $"01012{index:D4}",
                    MobileNumber = $"0912{index:D7}",
                    TrackNumber = 500000 + index,
                    Debt = 1_000_000L + (index * 1000L),
                    PostalCode = $"134567{index:D4}",
                    MainSiphonTitle = $"سیفون {index}",
                    MainSiphonId = index,
                    X = $"51.34{index:D4}",
                    Y = $"35.70{index:D4}",
                    DomesticUnit = (index % 4) + 1,
                    CommercialUnit = index % 2,
                    OtherUnit = index % 3,
                    DiscountId = index,
                    DiscountTitle = $"تخفیف {index}",
                    WaterDiameterId = index,
                    WaterDiameterTitle = $"قطر آب {index}",
                    WaterInstallationDateJalali = $"1398/08/{((index - 1) % 28) + 1:D2}",
                    SewageInstallationDateJalali = $"1400/03/{((index - 1) % 28) + 1:D2}",
                    GuildId = index,
                    GuildTitle = $"صنف {index}",
                    HouseholdUnit = (index % 10) + 1,
                    HouseholdDateJalali = $"1404/01/{((index - 1) % 28) + 1:D2}"
                })
                .ToArray();

            return Task.FromResult(readings);
        }
    }
}
