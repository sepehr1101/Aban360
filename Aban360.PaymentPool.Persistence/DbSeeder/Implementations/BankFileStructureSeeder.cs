using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.DbSeeder.Implementations
{
    public class BankFileStructureSeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<BankFileStructure> _bankFileStructure;
        public BankFileStructureSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _bankFileStructure = _uow.Set<BankFileStructure>();
            _bankFileStructure.NotNull(nameof(_bankFileStructure));
        }

        public void SeedData()
        {
            if (_bankFileStructure.Any())
            {
                return;
            }

            //sample to test
            ICollection<BankFileStructure> bankFileStructures = new List<BankFileStructure>()
            {
                new BankFileStructure(){FromIndex=0,ToIndex=7,StringLenght=8,Title="Bill",IsHeader=false},
                new BankFileStructure(){FromIndex=8,ToIndex=14,StringLenght=7,Title="Pay",IsHeader=false},
                new BankFileStructure(){FromIndex=15,ToIndex=28,StringLenght=14,Title="National",IsHeader=false},
                new BankFileStructure(){FromIndex=29,ToIndex=35,StringLenght=7,Title="Phone",IsHeader=false},
            };
            _bankFileStructure.AddRange(bankFileStructures);
            _uow.SaveChanges();
        }
    }
}
