using Data;
using Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Impl {

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user =  await _context.Users.FindAsync(id);

            if(user == null){
                return null;
            }

            return user;
        }

        public async Task<User?> AddUserAsync(User user) 
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}