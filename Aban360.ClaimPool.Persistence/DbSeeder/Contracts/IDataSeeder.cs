namespace Aban360.UserPool.Persistence.DbSeeder.Contracts
{
    internal interface IDataSeeder
    {
        int Order { set; get; }
        void SeedData();
    }
}
