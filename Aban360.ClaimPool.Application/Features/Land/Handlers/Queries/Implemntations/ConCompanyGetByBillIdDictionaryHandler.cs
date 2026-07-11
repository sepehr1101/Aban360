using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class ConCompanyGetByBillIdDictionaryHandler : IConCompanyGetByBillIdDictionaryHandler
    {
        private readonly ICommonMemberQueryService _memberQueryService;
        private readonly IConCompanyQueryService _conCompanyQueryService;
        public ConCompanyGetByBillIdDictionaryHandler(
               ICommonMemberQueryService memberQueryService,
               IConCompanyQueryService conCompanyQueryService)
        {
            _memberQueryService = memberQueryService;
            _memberQueryService.NotNull(nameof(memberQueryService));

            _conCompanyQueryService = conCompanyQueryService;
            _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));
        }
        public async Task<IEnumerable<NumericDictionary>> Handle(string billId, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _memberQueryService.Get(billId);
            IEnumerable<ConCompanyGetDto> conCompanyInfo = await _conCompanyQueryService.GetValidByZoneId(zoneIdAndCustomerNumber.ZoneId);
            IEnumerable<NumericDictionary> dictionary = conCompanyInfo.Select(c => new NumericDictionary(c.Id, c.CompanyName));
            return dictionary;
        }
    }
}
