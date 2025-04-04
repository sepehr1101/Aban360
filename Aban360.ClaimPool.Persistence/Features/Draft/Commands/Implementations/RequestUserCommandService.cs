using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Implementations
{
    internal sealed class RequestUserCommandService : IRequestUserCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestWaterMeter> _requestUser;
        public RequestUserCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestUser = _uow.Set<RequestWaterMeter>();
            _requestUser.NotNull(nameof(_requestUser));
        }

        public async Task Add(RequestUser requestUser)
        {
            //   await _requestUser.AddAsync(requestUser);
        }
        public async Task Add(RequestWaterMeter requestWaterMeter)
        {
            await _requestUser.AddAsync(requestWaterMeter);
        }
    }
}
