namespace Aban360.LocationPool.Persistence.DbSeeder.Contracts
{
    internal interface IDataSeeder
    {
        int Order { set; get; }
        void SeedData();
    }
}
