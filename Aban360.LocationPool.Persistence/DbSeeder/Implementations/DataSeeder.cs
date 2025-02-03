﻿using Aban360.Common.Extensions;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using DataSeeders = Aban360.Common.Db.DbSeeder.Contracts;

namespace Aban360.LocationPool.Persistence.DbSeeder.Implementations
{
    public class DataSeeder : DataSeeders.IDataSeeder
    {
        public int Order { get; set; } = 20;
        private readonly IUnitOfWork _uow;
        public DataSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
        }

        public void SeedData()
        {
            try
            {
                string sqlFilePath = GetSqlFilePath();
                if (!File.Exists(sqlFilePath))
                {
                    throw new FileNotFoundException($"فایل SQL یافت نشد: {sqlFilePath}");
                }

                string sqlScript = File.ReadAllText(sqlFilePath);

                ExecuteSqlScript(sqlScript);
                Console.WriteLine("داده‌ها با موفقیت اجرا شدند.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطایی رخ داد: {ex.Message}");
                throw;
            }
        }

        private void ExecuteSqlScript(string sqlScript)
        {
            _uow.ExecuteNonResultQuery(sqlScript).Wait();
        }
        
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"..\..\..\..\Aban360.LocationPool.Persistence\DbSeeder\DataScript\Aban360-Data-removeSpaceByTrim.sql";

            return Path.Combine(basePath, relativePath);
        }
    }
}
