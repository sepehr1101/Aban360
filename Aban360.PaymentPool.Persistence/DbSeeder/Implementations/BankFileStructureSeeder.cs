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
                new BankFileStructure(){FromIndex=20,ToIndex=28,StringLenght=9,Title="BillId",IsHeader=false,BankId=(short)1},
                new BankFileStructure(){FromIndex=34,ToIndex=45,StringLenght=12,Title="PaymentId",IsHeader=false,BankId=(short)1},
            };
            _bankFileStructure.AddRange(bankFileStructures);
            _uow.SaveChanges();
        }
    }
}
