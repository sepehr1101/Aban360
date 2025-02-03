using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Persistence.Contexts.Contracts;

namespace Aban360.LocationPool.Persistence.DbSeeder.Implementations
{
    public class MainHirearchySeeder : IDataSeeder
    {
        public int Order { get; set; } = 20;
        private readonly IUnitOfWork _uow;
        public MainHirearchySeeder(IUnitOfWork uow)
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
                var sqlCommands = sqlScript.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var command in sqlCommands.Select(cmd => cmd.Trim()).Where(cmd => !string.IsNullOrWhiteSpace(cmd)))
                {
                    if (command.Contains("IDENTITY_INSERT") || string.IsNullOrEmpty(command))
                    {
                        continue;
                    }

                    ExecuteSqlScript(command);
                }

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
            string relativePath = @"..\..\..\..\Aban360.LocationPool.Persistence\DbSeeder\DataScript\mainHierarchy.sql";

            return Path.Combine(basePath, relativePath);
        }
    }
}
