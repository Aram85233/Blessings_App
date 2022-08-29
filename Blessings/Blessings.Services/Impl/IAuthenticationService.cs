using Blessings.Data.Entities;

namespace Blessings.Services.Impl
{
    public interface IAuthenticationService
    {
        Task<bool> EmailUsedAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task<User> GetUserByEmailAndPasswordAsync(string email, string password);
        Task<string> SignInAsync(string email, string password);

    }
}
