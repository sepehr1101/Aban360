using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.DbSeeder.Implementations
{
    public class AccessTreeSeeder:IDataSeeder
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<App> _apps;
        public AccessTreeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow)); 

            _apps=_uow.Set<App>();
            _apps.NotNull(nameof(_apps));
        }
        public int Order { get; set; } = 1;

        public void SeedData()
        {
            if (_apps.Any())
            {
                return;
            }
            var app = GetApp();
            _apps.Add(app);
            _uow.SaveChanges();
        }

        private App GetApp()
        {
            var app = new App()
            {
                InMenu = true,
                IsActive = true,
                LogicalOrder = 1,
                Style=string.Empty,
                Title="دسترسی ها",
                Modules=GetModules()
            };
            return app;
        }
        private ICollection<Module> GetModules()
        {
            var modules = new List<Module>()
            {
                new Module()
                {
                    ClientRoute="client/route/captcha",
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="تصاویر امنیتی",
                    SubModules=GetSubModules()
                }
            };
            return modules;
        }
        private ICollection<SubModule> GetSubModules()
        {
            var subModules = new List<SubModule>()
            {
                new SubModule()
                {
                    ClientRoute="sub-module-route",
                    Endpoints=GetEndpoints(),
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="زیر ماژول آزمایشی"
                }
            };
            return subModules;
        }
        private ICollection<Endpoint> GetEndpoints()
        {
            var endpoints = new List<Endpoint>()
            {
                new Endpoint()
                {
                    AuthValue="authVal",
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="عنوان آزمایشی"
                }
            };
            return endpoints;
        }
    }
}
