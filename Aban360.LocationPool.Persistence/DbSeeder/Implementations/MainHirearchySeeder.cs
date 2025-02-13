using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.DbSeeder.Implementations
{
    public class MainHirearchySeeder : IDataSeeder
    {        
        public int Order { get; set; } = 20;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Country> _countries;
        public MainHirearchySeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _countries = _uow.Set<Country>();
            _countries.NotNull(nameof(_countries));
        }
        public void SeedData()
        {
            if(_countries.Any())
            {
                return;
            }
            string sqlFilePath = GetSqlFilePath();
            _uow.ExecuteBatch(sqlFilePath);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\mainHierarchy.sql";

            var path= string.Concat(basePath, relativePath);
            return path;
        }
    }
}
