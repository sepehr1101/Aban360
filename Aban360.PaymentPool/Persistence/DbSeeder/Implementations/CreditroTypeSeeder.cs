using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.DbSeeder.Implementations
{
    public class CreditroTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CreditorType> _creditorType;
        public CreditroTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _creditorType = _uow.Set<CreditorType>();
            _creditorType.NotNull(nameof(_creditorType));
        }

        public void SeedData()
        {
            if (_creditorType.Any())
            {
                return;
            }

            ICollection<CreditorType> creditroTypes = new List<CreditorType>()
            {
                new CreditorType(){Id=CreditorTypeEnum.ElectronicPayment,Title="پرداخت الکترونیک"},    
                new CreditorType(){Id=CreditorTypeEnum.ManualRegistration,Title="ثبت دستی وصولی"},    
                new CreditorType(){Id=CreditorTypeEnum.Barter,Title="تهاتر"},    
                new CreditorType(){Id=CreditorTypeEnum.Cheque,Title="چک"},    
                new CreditorType(){Id=CreditorTypeEnum.InstallmentBook,Title="دفترچه قسط"},    
                new CreditorType(){Id=CreditorTypeEnum.DemandNote,Title="سفته"},    
            };

            _creditorType.AddRange(creditroTypes);
            _uow.SaveChanges();
        }
    }
}
