using GuardianBackend.Domain.Entities;
using GuardianBackend.Infrastructure.Data;
using GuardianBackend.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GuardianBackend.Tests._3_Repository
{
    public class UserRepositoryTests
    {
        private readonly GuardianDbContext _context;
        private readonly UserRepository _repo;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<GuardianDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new GuardianDbContext(options);
            _repo = new UserRepository(_context);
        }

        [Fact]
        public void GetAllUsers_ReturnsListOfUsers()
        {
            var user1 = new User { Id = 1, Username = "Usuário de Teste 1" };
            var user2 = new User { Id = 2, Username = "Usuário de Teste 2" };

            _context.Users.AddRange(user1, user2);
            _context.SaveChanges();

            var users = _repo.GetAllUsers();

            Assert.NotEmpty(users);
        }

        [Fact]
        public void GetUserById_ReturnsNull_WhenUserDoesNotExistInDb()
        {
            var user = _repo.GetUserById(1);

            Assert.Null(user);
        }

        [Fact]
        public void GetUserById_ReturnsUser_WhenUserExistsInDb()
        {
            var testUser = new User { Id = 1, Username = "Usuário de Teste" };
            _context.Users.Add(testUser);
            _context.SaveChanges();

            var user = _repo.GetUserById(1);

            Assert.NotNull(user);
            Assert.Equal(testUser.Id, user.Id);
        }
    }
}
