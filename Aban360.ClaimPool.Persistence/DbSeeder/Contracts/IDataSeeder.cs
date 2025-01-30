namespace Aban360.ClaimPool.Persistence.DbSeeder.Contracts
{
    internal interface IDataSeeder
    {
        int Order { set; get; }
        void SeedData();
    }
}
