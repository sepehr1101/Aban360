using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class GatewaySeeder : IDataSeeder
    {
        public int Order { get; set; } = 29;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Gateway> _gateways;
        public GatewaySeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _gateways = _uow.Set<Gateway>();
            _gateways.NotNull(nameof(_gateways));
        }

        public void SeedData()
        {
            if (_gateways.Any())
            {
                return;
            }

            ICollection<Gateway> gateways = new List<Gateway>()
            {
                  new Gateway() { Id=1,Title="پنل سامانه آبان"},
                  new Gateway() { Id=2,Title="BPMS"},
                  new Gateway() { Id=3,Title="همراه آبفا"},
                  new Gateway() { Id=4,Title="پنجره واحد"},
                  new Gateway() { Id=5,Title="سامانه پیامکی"},
                  new Gateway() { Id=6,Title="USSD"},
                  new Gateway() { Id=7,Title="سامانه تلفنی"},
                  new Gateway() { Id=8,Title="پرتال شرکت"},
            };

            _gateways.AddRange(gateways);
            _uow.SaveChanges();
        }
    }
}
