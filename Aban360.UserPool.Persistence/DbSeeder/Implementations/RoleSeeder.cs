using Aban360.Common.Categories.UseragentLog;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.Common.Db.DbSeeder.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.DbSeeder.Implementations
{
    public class RoleSeeder: IDataSeeder
    {
        public int Order { get; set; } = 5;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Role> _roles;
        public RoleSeeder(IUnitOfWork uow)
        {
            _uow= uow;
            _uow.NotNull(nameof(uow));

            _roles=_uow.Set<Role>();
            _roles.NotNull(nameof(_roles));
        }

        public void SeedData()
        {
            if (!_roles.Any())
            {
                Role admin = CreateRole(BaseRoles.Admin, "راهبر سامانه");
                Role ai = CreateRole(BaseRoles.Ai, "دستیار هوشمند");
                Role programmer = CreateRole(BaseRoles.Programmer, "برنامه نویس");
                Role headOfSubscribersDepartment = CreateRole(BaseRoles.HeadOfSubscribersDepartment, "رئیس اداره مشترکین");
                Role receivablesResponsible = CreateRole(BaseRoles.ReceivablesResponsible, "مسئول وصول مطالبات");
                Role detectionAndDistributionResponsible = CreateRole(BaseRoles.DetectionAndDistributionResponsible, "مسئول تشخیص و توزیع");
                Role hotline1522Reception = CreateRole(BaseRoles.Hotline1522Reception, "رابط 1522/پذیرش");
                Role receivablesSpecialist = CreateRole(BaseRoles.ReceivablesSpecialist, "کارشناس وصول مطالبات");
                Role detectionAndDistributionSpecialist = CreateRole(BaseRoles.DetectionAndDistributionSpecialist, "کارشناس تشخیص و توزیع");
                Role reviewer = CreateRole(BaseRoles.Reviewer, "بازبین");
                Role serviceDesk = CreateRole(BaseRoles.ServiceDesk, "میز خدمت");
                Role evaluator = CreateRole(BaseRoles.Evaluator, "ارزیاب");

                Role[] roles =
                [
                    admin,
                    ai,
                    programmer,
                    headOfSubscribersDepartment,
                    receivablesResponsible,
                    detectionAndDistributionResponsible,
                    hotline1522Reception,
                    receivablesSpecialist,
                    detectionAndDistributionSpecialist,
                    reviewer,
                    serviceDesk,
                    evaluator
                ];
                _roles.AddRange(roles);
                _uow.SaveChanges();
            }
        }

        public Role CreateRole(string name, string title)
        {
            return new Role()
            {
                Hash = string.Empty,
                InsertLogInfo = LogInfoJson.Get(),
                IsRemovable = false,
                SensitiveInfo = false,
                ValidFrom = DateTime.Now,
                Name = name,
                Title = title
            };
        }
    }
}
