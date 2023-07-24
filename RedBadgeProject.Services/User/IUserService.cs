using RedBadgeProject.Models.User;

namespace RedBadgeProject.Services.User
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserRegister model);
        Task<bool> LoginAsync(UserLogin model);
        Task<bool> LogoutAsync();
    }
}