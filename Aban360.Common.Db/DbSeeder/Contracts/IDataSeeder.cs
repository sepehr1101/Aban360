namespace Aban360.Common.Db.DbSeeder.Contracts
{
    public interface IDataSeeder
    {
        int Order { set; get; }
        void SeedData();
    }
}
