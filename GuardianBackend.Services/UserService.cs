using GuardianBackend.Domain.Entities;
using GuardianBackend.Domain.Interfaces;

namespace GuardianBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers() => _userRepository.GetAllUsers();

        public User? GetUserById(int id) => _userRepository.GetUserById(id);

    }
}
