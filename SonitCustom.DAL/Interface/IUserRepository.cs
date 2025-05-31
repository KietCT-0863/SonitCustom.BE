using SonitCustom.DAL.Entities;

namespace SonitCustom.DAL.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUserAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task AddNewUserAsync(User newUser);
        Task UpdateUserAsync(User userToUpdate);
        Task<User?> GetUserAsync(string username, string password);
        Task<string?> GetRoleByUserIdAsync(int userId);
        Task<bool> CheckUserExistsAsync(string username, string email);
        Task<bool> IsUserNameExistsAsync(string username, int excludeUserId = 0);
        Task<bool> IsEmailExistsAsync(string email, int excludeUserId = 0);
        Task DeleteUserAsync(User userToDelete);
    }
}