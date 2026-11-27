namespace Aban360.Common.ApplicationUser;

public interface IOperationLogUser
{
    Guid Id { get; }
    string FullName { get; }
    string Username { get; }
}
