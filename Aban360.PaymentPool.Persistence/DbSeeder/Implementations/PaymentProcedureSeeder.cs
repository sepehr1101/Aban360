using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.DbSeeder.Implementations
{
    public class PaymentProcedureSeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<PaymentProcedure> _paymentProcedures;
        public PaymentProcedureSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _paymentProcedures = _uow.Set<PaymentProcedure>();
            _paymentProcedures.NotNull(nameof(_paymentProcedures));
        }

        public void SeedData()
        {
            if (_paymentProcedures.Any())
            {
                return;
            }

            ICollection<PaymentProcedure> paymentProcedures = new List<PaymentProcedure>()
            {
                new PaymentProcedure(){Id=PaymentProcedureEnum.PresenceAtBank,Title="حضور در شعبه",Icon="",Description="" },
                new PaymentProcedure(){Id=PaymentProcedureEnum.Internet,Title="اینترنت",Icon="",Description="" },
                new PaymentProcedure(){Id=PaymentProcedureEnum.PhoneBank,Title="تلفن بانک",Icon="",Description="" },
            };

            _paymentProcedures.AddRange(paymentProcedures);
            _uow.SaveChanges();
        }
    }
}
