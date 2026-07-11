using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using System.Text.Json;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class ConCompanyPersonnelGetDictionaryHandler : IConCompanyPersonnelGetDictionaryHandler
    {
        private readonly IConCompanyQueryService _conCompanyQueryService;
        public ConCompanyPersonnelGetDictionaryHandler(IConCompanyQueryService conCompanyQueryService)
        {
            _conCompanyQueryService = conCompanyQueryService;
            _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));
        }
        public async Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<ConCompanyPersonnelGetDto> conCompanyPersonnelInfo = await _conCompanyQueryService.GetPersonnel();

            ICollection<NumericDictionary> resultDictionary = new List<NumericDictionary>();
            conCompanyPersonnelInfo.ForEach(c =>
            {
                var personnels = JsonSerializer.Deserialize<IEnumerable<ConCompanyPersonnelDetailOutputDto>>(c.ConCompanyPersonnel);
                IEnumerable<NumericDictionary> dictionary = personnels?.Select(p => new NumericDictionary(c.Id, $@"{p.FullName}-{c.CompanyName}")) ?? Enumerable.Empty<NumericDictionary>();
                resultDictionary = resultDictionary.Concat(dictionary.ToList()).ToList();
            });

            return resultDictionary;
        }
    }
}
