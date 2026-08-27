namespace Application.Contracts.UserManagement
{
    public interface ICurrentUserRepository
    {
        Guid GetCurrentLoggedInUserId();
    }
}