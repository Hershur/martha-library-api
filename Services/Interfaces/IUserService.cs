using Models;

namespace Services.Interfaces {
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> AddUserAsync(User user); 
    }
}