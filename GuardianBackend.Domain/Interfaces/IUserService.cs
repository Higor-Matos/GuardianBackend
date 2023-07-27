using GuardianBackend.Common;
using GuardianBackend.Common.Attributes;
using GuardianBackend.Domain.Entities;

namespace GuardianBackend.Domain.Interfaces
{
    [AutoDI]
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User? GetUserById(int id);
    }
}
