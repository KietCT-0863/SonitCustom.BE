using SonitCustom.DAL.Entities;

namespace SonitCustom.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUserAsync();
        Task AddNewUserAsync(User newUser);
        Task UpdateUserAsync(User userToUpdate);
        Task<User?> GetUserAsync(string username, string password);
        Task<string?> GetRoleByUserIdAsync(int userId);
        Task<bool> CheckUserExistsAsync(string username, string email);
    }
}