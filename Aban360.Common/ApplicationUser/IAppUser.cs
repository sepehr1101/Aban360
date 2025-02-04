namespace Aban360.Common.ApplicationUser
{
    public interface IAppUser
    {
        public Guid UserId { get; }
        public string Username { get; }
        public string FullName { get; }
        public ICollection<string> Roles { get; }
        public bool IsAdmin { get; }
    }
}
