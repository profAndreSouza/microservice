using Microsoft.EntityFrameworkCore;
using UserAuth.Domain.Entities;
using UserAuth.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserAuth.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            // Incluindo as roles na busca de usuários
            return await _context.Users.Include(u => u.Roles).ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            // Incluindo as roles na busca de um usuário específico
            return await _context.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Id == id);
        }


        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Include(u => u.Roles)
                                    .SingleOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> FindUserByUsername(string username)
        {
            return await _context.Users.Include(u => u.Roles)
                                    .SingleOrDefaultAsync(u => u.Username == username);
        }


        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Role>> GetRolesByUserId(int userId)
        {
            var user = await _context.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Id == userId);
            return user?.Roles ?? new List<Role>();
        }

        public async Task AddRoleToUser(int userId, Role role)
        {
            var user = await _context.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Id == userId);
            if (user != null && !user.Roles.Contains(role))
            {
                user.Roles.Add(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveRoleFromUser(int userId, int roleId)
        {
            var user = await _context.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                var roleToRemove = user.Roles.SingleOrDefault(r => r.Id == roleId);
                if (roleToRemove != null)
                {
                    user.Roles.Remove(roleToRemove);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
