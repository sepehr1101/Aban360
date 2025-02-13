using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    public class ProvienceQueryAddhoc : IProvienceQueryAddhoc
    {
        private readonly IProvinceGetAllHandler _provienceGetHandler;
        public ProvienceQueryAddhoc(IProvinceGetAllHandler provienceGetHandler)
        {
            _provienceGetHandler = provienceGetHandler;
            _provienceGetHandler.NotNull(nameof(provienceGetHandler));
        }

        public async Task<ICollection<NumericDictionary>> Get(CancellationToken cancellationToken)
        {
            var provience = await _provienceGetHandler.Handle(cancellationToken);
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
