using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class ChangeMeterReasonSeeder : IDataSeeder
    {
        public int Order { get; set; } = 24;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ChangeMeterReason> _changeMeterReasons;
        public ChangeMeterReasonSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _changeMeterReasons = _uow.Set<ChangeMeterReason>();
            _changeMeterReasons.NotNull(nameof(_changeMeterReasons));
        }

        public void SeedData()
        {
            if (_changeMeterReasons.Any())
            {
                return;
            }

            ICollection<ChangeMeterReason> changeMeterReason = new List<ChangeMeterReason>()
            {
                new ChangeMeterReason(){Id=ChangeMeterReasonEnum.Reading,Title="قرائت"},
                new ChangeMeterReason(){Id=ChangeMeterReasonEnum.Change,Title="تعویض کنتور"},
            };
            _changeMeterReasons.AddRange(changeMeterReason);
            _uow.SaveChanges();
        }
    }
}
