using Microsoft.EntityFrameworkCore;
using Shop.Data.Data;
using Shop.Data.Helpers;
using Shop.Data.Models;

namespace Shop.Data.Services
{
    public class UserService
    {
        private readonly ShopDbContext _context;

        public UserService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return null; 

            if (!user.IsActive) return null; 

            bool isValid = PasswordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);

            if (!isValid) return null; 
            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> RegisterUserAsync(string username, string password, string email, string role = "User")
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                return false; 
            }

            var (hash, salt) = PasswordHasher.HashPassword(password);

            var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = role,
                IsActive = true,
                LastLogin = null
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return true;
        }
        

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

    public async Task<bool> UpdateUserAsync(User userToUpdate)
{
    var existingUser = await _context.Users.FindAsync(userToUpdate.Id);
    if (existingUser == null) return false;

 
    existingUser.Username = userToUpdate.Username;
    existingUser.Email = userToUpdate.Email;
    existingUser.Role = userToUpdate.Role;

   
    if (!string.IsNullOrEmpty(userToUpdate.PasswordHash))
    {
        existingUser.PasswordHash = userToUpdate.PasswordHash;
        existingUser.PasswordSalt = userToUpdate.PasswordSalt;
    }

    _context.Users.Update(existingUser);
    await _context.SaveChangesAsync();
    return true;
}
        public async Task InitAdminAsync()
        {
          
            var adminExists = await _context.Users.AnyAsync(u => u.Username == "admin");

            if (!adminExists)
            {
                var (hash, salt) = PasswordHasher.HashPassword("Admin123!");

                var admin = new User
                {
                    Username = "admin",
                    Email = "admin@SportX.com",
                    Role = "Admin", 
                    IsActive = true,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    LastLogin = null
                };

                _context.Users.Add(admin);
                await _context.SaveChangesAsync();
            }
        }
    }
}