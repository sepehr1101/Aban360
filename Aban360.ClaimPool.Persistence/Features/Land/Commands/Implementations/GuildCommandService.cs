using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public class GuildCommandService : IGuildCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Guild> _guild;
        public GuildCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _guild = _uow.Set<Guild>();
            _guild.NotNull(nameof(_guild));
        }

        public async Task Add(Guild guild)
        {
            await _guild.AddAsync(guild);
        }

        public async Task Remove(Guild guild)
        {
            _guild.Remove(guild);
        }
    }
}
