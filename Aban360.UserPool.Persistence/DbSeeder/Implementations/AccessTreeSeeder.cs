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

            string sqlFilePath = GetSqlFilePath();
            _uow.ExecuteBatch(sqlFilePath);

            // var app = GetApp();
            // _apps.Add(app);
            // _uow.SaveChanges();
        }

        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\AccessTree-1404_02_20.sql";

            var path = string.Concat(basePath, relativePath);
            return path;
        }


        private App GetApp()
        {
            var app = new App()
            {
                InMenu = true,
                IsActive = true,
                LogicalOrder = 1,
                Style=string.Empty,
                Title="امنیت و کاربران",
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
                    ClientRoute=string.Empty,
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="تصاویر امنیتی",
                    SubModules=GetSubModulesCaptcha()
                },
                 new Module()
                {
                    ClientRoute=string.Empty,
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="مدیریت کاربران",
                    SubModules=GetSubModulesUserManagement()
                }
            };
            return modules;
        }
        private ICollection<SubModule> GetSubModulesCaptcha()
        {
            var subModules = new List<SubModule>()
            {
                new SubModule()
                {
                    ClientRoute="v1/captcha/show1",
                    Endpoints=GetEndpointsCaptcha(),
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="زیر ماژول کپچا 1"
                },
                new SubModule()
                {
                    ClientRoute="v1/captcha/show2",
                    Endpoints=GetEndpointsCaptcha(),
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="زیر ماژول کپچا2"
                },
                new SubModule()
                {
                    ClientRoute="v1/captcha/show3",
                    Endpoints=GetEndpointsCaptcha(),
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="زیر ماژول کپچا3"
                },
                new SubModule()
                {
                    ClientRoute="v1/captcha/show4",
                    Endpoints=GetEndpointsCaptcha(),
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="زیر ماژول کپچا4"
                }
            };
            return subModules;
        }
        private ICollection<SubModule> GetSubModulesUserManagement()
        {
            var subModules = new List<SubModule>()
            {
                new SubModule()
                {
                    ClientRoute="v1/user/all",
                    Endpoints=GetEndpointsUsers(),
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="زیر ماژول کاربران"
                }
            };
            return subModules;
        }
        private ICollection<Endpoint> GetEndpointsCaptcha()
        {
            var endpoints = new List<Endpoint>()
            {                
                 new Endpoint()
                {
                    AuthValue=@"CaptchaDictionary.Get",
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="مشاهده"
                },
                  new Endpoint()
                {
                    AuthValue=@"CaptchaLanguage.Get",
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="مشاهده"
                },
                   new Endpoint()
                {
                    AuthValue=@"CaptchaDisplayMode.Get",
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="مشاهده"
                }
            };
            return endpoints;
        }
        private ICollection<Endpoint> GetEndpointsUsers()
        {
            var endpoints = new List<Endpoint>()
            {
                new Endpoint()
                {
                    AuthValue=@"UserCreate.Trigger",
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=1,
                    Style=string.Empty,
                    Title="افزودن کاربر"
                },
                 new Endpoint()
                {
                    AuthValue=@"UserManager.All",
                    InMenu=true,
                    IsActive=true,
                    LogicalOrder=2,
                    Style=string.Empty,
                    Title="نمایش همه"
                }
            };
            return endpoints;
        }

    }
}
