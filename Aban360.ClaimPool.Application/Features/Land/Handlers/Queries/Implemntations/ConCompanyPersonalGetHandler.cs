using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using System.Text.Json;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class ConCompanyPersonalGetHandler : IConCompanyPersonalGetHandler
    {
        private readonly IConCompanyQueryService _conCompanyQueryService;
        public ConCompanyPersonalGetHandler(IConCompanyQueryService conCompanyQueryService)
        {
            _conCompanyQueryService = conCompanyQueryService;
            _conCompanyQueryService.NotNull(nameof(conCompanyQueryService));
        }
        public async Task<IEnumerable<ConCompanyPersonnelDetailOutputDto>> Handle(int id, CancellationToken cancellationToken)
        {
            ConCompanyPersonnelGetDto result = await _conCompanyQueryService.GetPersonnel(id);
            if (!string.IsNullOrWhiteSpace(result.ConCompanyPersonnel))
            {
                IEnumerable<ConCompanyPersonnelDetailOutputDto> personnelInfo = JsonSerializer.Deserialize<IEnumerable<ConCompanyPersonnelDetailOutputDto>>(result.ConCompanyPersonnel);
                return personnelInfo;
            }

            throw new InvalidTrackingException(ExceptionLiterals.InvalidConCompanyPersonnelDeserialize);
        }
    }
}
