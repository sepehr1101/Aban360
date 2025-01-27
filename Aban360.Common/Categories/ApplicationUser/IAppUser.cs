namespace Aban360.Common.Categories.ApplicationUser
{
    public interface IAppUser
    {
        public Guid UserId { get; }
        public string Username { get; }
        public int UserCode { get; }
        public string DisplayName { get; }
        public ICollection<string> Roles { get; }
    }
}
