using GuardianBackend.Domain.Entities;
using GuardianBackend.Domain.Interfaces;
using GuardianBackend.Services;
using Moq;
using Xunit;

namespace GuardianBackend.Tests._2_Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _service = new UserService(_mockRepo.Object);
        }

        [Fact]
        public void GetAllUsers_CallsRepositoryMethod()
        {
            _mockRepo.Setup(repo => repo.GetAllUsers()).Returns(new List<User>());
            _service.GetAllUsers();
            _mockRepo.Verify(repo => repo.GetAllUsers(), Times.Once);
        }

        [Fact]
        public void GetUserById_ReturnsNull_WhenUserDoesNotExist()
        {
            int testUserId = 1;
            _mockRepo.Setup(repo => repo.GetUserById(testUserId)).Returns((User?)null);
            User? user = _service.GetUserById(testUserId) as User;
            Assert.Null(user);
        }

        [Fact]
        public void GetUserById_ReturnsUser_WhenUserExists()
        {
            int testUserId = 1;
            _mockRepo.Setup(repo => repo.GetUserById(testUserId)).Returns(new User { Id = testUserId });
            var user = _service.GetUserById(testUserId);
            Assert.NotNull(user);
            Assert.Equal(testUserId, user.Id);
        }
    }
}
