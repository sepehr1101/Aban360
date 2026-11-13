using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    internal sealed class ProvienceQueryAddhoc : IProvienceQueryAddhoc
    {
        private readonly IProvinceService _service;

        public ProvienceQueryAddhoc(IProvinceService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        public async Task<ICollection<NumericDictionary>> Get(CancellationToken cancellationToken)
        {
            ICollection<ProvinceGetDto> provience = await _service.GetAllAsync(cancellationToken);
            return provience
                .Select(p => new NumericDictionary()
                {
                    Id = p.Id,
                    Title = p.Title,
                })
                .ToList();
        }
    }
}
