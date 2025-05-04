using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Constansts;
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

            ICollection<BankFileStructure> bankFileStructures = new List<BankFileStructure>()
            {
                //Header
                new BankFileStructure(){FromIndex=0,ToIndex=1,StringLenght=1,Title="کد شرکت خدماتی",IsHeader=true,BankStructureItemId=BankStructureItemEnum.UtilityCode},
                new BankFileStructure(){FromIndex=1,ToIndex=4,StringLenght=3,Title="کد شرکت تابعه",IsHeader=true,BankStructureItemId=BankStructureItemEnum.SubUtilityCode},
                new BankFileStructure(){FromIndex=4,ToIndex=6,StringLenght=2,Title="کد بانک",IsHeader=true,BankStructureItemId=BankStructureItemEnum.BankCode},
                new BankFileStructure(){FromIndex=6,ToIndex=12,StringLenght=6,Title="تاریخ ارسال فایل",IsHeader=true,BankStructureItemId=BankStructureItemEnum.SendData},
                new BankFileStructure(){FromIndex=12,ToIndex=22,StringLenght=10,Title="جمع مبلغ قبوض مندرج در فایل",IsHeader=true,BankStructureItemId=BankStructureItemEnum.TotalPrice},
                new BankFileStructure(){FromIndex=22,ToIndex=30,StringLenght=8,Title="تعداد سطرها",IsHeader=true,BankStructureItemId=BankStructureItemEnum.RecordNO},

                //Row
                new BankFileStructure(){FromIndex=0,ToIndex=6,StringLenght=6,Title="کد شعبه دریافت کننده قبض",IsHeader=false,BankStructureItemId=BankStructureItemEnum.BranchCode},
                new BankFileStructure(){FromIndex=6,ToIndex=8,StringLenght=2,Title="روش پرداخت",IsHeader=false,BankStructureItemId=BankStructureItemEnum.ChannelType},
                new BankFileStructure(){FromIndex=8,ToIndex=14,StringLenght=6,Title="تاریخ پرداخت قبض",IsHeader=false,BankStructureItemId=BankStructureItemEnum.PayDate},
                new BankFileStructure(){FromIndex=14,ToIndex=27,StringLenght=13,Title="شناسه قبض",IsHeader=false,BankStructureItemId=BankStructureItemEnum.BillID},
                new BankFileStructure(){FromIndex=27,ToIndex=40,StringLenght=13,Title="شناسه پرداخت",IsHeader=false,BankStructureItemId=BankStructureItemEnum.PaymentID},
                new BankFileStructure(){FromIndex=40,ToIndex=46,StringLenght=6,Title="شماره پیگیری",IsHeader=false,BankStructureItemId=BankStructureItemEnum.RefCode},
            };
            _bankFileStructure.AddRange(bankFileStructures);
            _uow.SaveChanges();
        }
    }
}
