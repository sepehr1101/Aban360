using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    public class ChangeMeterReasonQueryService : IChangeMeterReasonQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ChangeMeterReason> _changeMeterReason;
        public ChangeMeterReasonQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _changeMeterReason = _uow.Set<ChangeMeterReason>();
            _changeMeterReason.NotNull(nameof(_changeMeterReason));
        }

        public async Task<ChangeMeterReason> Get(ChangeMeterReasonEnum id)
        {
            return await _uow.FindOrThrowAsync<ChangeMeterReason>(id);
        }

        public async Task<ICollection<ChangeMeterReason>> Get()
        {
            return await _changeMeterReason.ToListAsync();
        }
    }
}
