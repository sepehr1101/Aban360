using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public class EstateBoundTypeCommandService : IEstateBoundTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EstateBoundType> _estateBoundType;
        public EstateBoundTypeCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _estateBoundType = _uow.Set<EstateBoundType>();
            _estateBoundType.NotNull(nameof(_estateBoundType));
        }

        public async Task Add(EstateBoundType estateBoundType)
        {
            await _estateBoundType.AddAsync(estateBoundType);
        }

        public async Task Remove(EstateBoundType estateBoundType)
        {
            _estateBoundType.Remove(estateBoundType);
        }
    }
}
