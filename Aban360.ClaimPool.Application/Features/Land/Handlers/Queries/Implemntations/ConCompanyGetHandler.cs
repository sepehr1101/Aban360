using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class ConCompanyGetHandler : IConCompanyGetHandler
    {
        private readonly IConCompanyQueryService _conCompanyQueryService;
        public ConCompanyGetHandler(IConCompanyQueryService conCompanyQueryService)
        {
            _conCompanyQueryService = conCompanyQueryService;
            _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));
        }
        public async Task<IEnumerable<ConCompanyGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<ConCompanyGetDto> result = await _conCompanyQueryService.Get();
            return result;
        }
    }
}