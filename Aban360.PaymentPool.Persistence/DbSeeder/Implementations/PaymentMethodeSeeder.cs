using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.DbSeeder.Implementations
{
    public class PaymentMethodeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<PaymentMethod> _paymentMethod;
        public PaymentMethodeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _paymentMethod = _uow.Set<PaymentMethod>();
            _paymentMethod.NotNull(nameof(_paymentMethod));
        }

        public void SeedData()
        {
            if (_paymentMethod.Any())
            {
                return;
            }

            ICollection<PaymentMethod> paymentMethods = new List<PaymentMethod>()
            {
                new PaymentMethod(){Id=PaymentMethodEnum.ATM,Title="خودپرداز",Icon="",Description="" },
                new PaymentMethod(){Id=PaymentMethodEnum.PresenceAtBank,Title="حضور در شعبه",Icon="",Description="" },
                new PaymentMethod(){Id=PaymentMethodEnum.Interanet,Title="اینترانت",Icon="",Description="" },
                new PaymentMethod(){Id=PaymentMethodEnum.SMS,Title="پیامک",Icon="",Description="" },
                new PaymentMethod(){Id=PaymentMethodEnum.BankPhone,Title="تلفن بانک",Icon="",Description="" },
                new PaymentMethod(){Id=PaymentMethodEnum.CRS,Title="خود دریافت",Icon="",Description="" },
                new PaymentMethod(){Id=PaymentMethodEnum.PaymentBank,Title="بانک دریافت",Icon="",Description="" },
                new PaymentMethod(){Id=PaymentMethodEnum.WebKiosk,Title="وب کیوسک",Icon="",Description="" },
                new PaymentMethod(){Id=PaymentMethodEnum.POS,Title="پایانه فروش",Icon="",Description="" },
                new PaymentMethod(){Id=PaymentMethodEnum.Internet,Title="اینترنت",Icon="",Description="" },
            };

            _paymentMethod.AddRange(paymentMethods);
            _uow.SaveChanges();
        }
    }
}