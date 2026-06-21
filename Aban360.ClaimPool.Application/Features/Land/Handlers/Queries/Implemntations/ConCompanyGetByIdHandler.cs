using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class ConCompanyGetByIdHandler : IConCompanyGetByIdHandler
    {
        private readonly IConCompanyQueryService _conCompanyQueryService;
        public ConCompanyGetByIdHandler(IConCompanyQueryService conCompanyQueryService)
        {
            _conCompanyQueryService = conCompanyQueryService;
            _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));
        }
        public async Task<ConCompanyGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            ConCompanyGetDto result = await _conCompanyQueryService.Get(id);
            return result;
        }
    }
}
