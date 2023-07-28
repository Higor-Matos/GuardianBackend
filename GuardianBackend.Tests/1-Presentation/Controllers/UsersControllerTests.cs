using GuardianBackend.Domain.Entities;
using GuardianBackend.Domain.Interfaces;
using GuardianBackend.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GuardianBackend.Tests._1_Presentation.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UsersController(_mockService.Object);
        }

        [Fact]
        public void GetAllUsers_RetornaOkResult_ComListaDeUsuarios()
        {
            _mockService.Setup(service => service.GetAllUsers()).Returns(new List<User>() { new User(), new User() });
            var result = _controller.GetAllUsers();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetUserById_RetornaNotFoundResult_QuandoUsuarioNaoExiste()
        {
            int testUserId = 1;
            _mockService.Setup(service => service.GetUserById(testUserId)).Returns((User?)null);
            var result = _controller.GetUserById(testUserId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetUserById_RetornaOkResult_QuandoUsuarioExiste()
        {
            int testUserId = 1;
            _mockService.Setup(service => service.GetUserById(testUserId)).Returns(new User { Id = testUserId });
            var result = _controller.GetUserById(testUserId);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
