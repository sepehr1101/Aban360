using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Repositories
{
    internal sealed class RequestUserRepository : IRequestUserCommandService, IRequestUserQueryService
    {
        private readonly IUnitOfWork _uow;
        // برای حفظ رفتار overload قدیمی Add(RequestWaterMeter)، این DbSet مستقل نگه داشته شده است.
        private readonly DbSet<RequestWaterMeter> _requestUser;
        private readonly DbSet<RequestUser> _requestUsers;
        public RequestUserRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestUser = _uow.Set<RequestWaterMeter>();
            _requestUser.NotNull(nameof(_requestUser));

            _requestUsers = _uow.Set<RequestUser>();
            _requestUsers.NotNull(nameof(_requestUsers));
        }

        public async Task Add(RequestUser requestUser)
        {
            //   await _requestUser.AddAsync(requestUser);
        }
        public async Task Add(RequestWaterMeter requestWaterMeter)
        {
            await _requestUser.AddAsync(requestWaterMeter);
        }

        public async Task<RequestUser> Get(short id)
        {
            return await _uow.FindOrThrowAsync<RequestUser>(id);
        }

        public async Task<ICollection<RequestUser>> Get()
        {
            return await _requestUsers.ToListAsync();
        }

    }
}
