namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IClientsCommandService
    {
        Task InsertByArchMemId(int id, string dbName);
    }
}
