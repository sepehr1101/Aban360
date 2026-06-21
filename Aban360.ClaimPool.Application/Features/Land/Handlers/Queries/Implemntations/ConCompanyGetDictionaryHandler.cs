using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class ConCompanyGetDictionaryHandler : IConCompanyGetDictionaryHandler
    {
        private readonly IConCompanyQueryService _conCompanyQueryService;
        public ConCompanyGetDictionaryHandler(IConCompanyQueryService conCompanyQueryService)
        {
            _conCompanyQueryService = conCompanyQueryService;
            _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));
        }
        public async Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<ConCompanyGetDto> conCompanyInfo = await _conCompanyQueryService.Get();
            IEnumerable<NumericDictionary> dictionary = conCompanyInfo.Select(c => new NumericDictionary(c.Id, c.CompanyName));
            return dictionary;
        }
    }
}
