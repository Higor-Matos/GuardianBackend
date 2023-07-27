using GuardianBackend.Domain.Entities;
using GuardianBackend.Domain.Interfaces;
using GuardianBackend.Infrastructure.Data;

namespace GuardianBackend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly GuardianDbContext _context;

        public UserRepository(GuardianDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers() => _context.Users.ToList();

        public User? GetUserById(int id) => _context.Users.Find(id);
    }
}
