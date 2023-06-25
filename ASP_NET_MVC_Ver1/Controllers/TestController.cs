using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class TestController : Controller
    {
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private Mock<IPasswordHasher<ApplicationUser>> mockPasswordHasher;
        private AdminController? adminController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var users = new List<ApplicationUser>()
        {
            new ApplicationUser() { Id = "1", FirstName = "John" },
            new ApplicationUser() { Id = "2", FirstName = "Jane" }
        }.AsQueryable();

            mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            mockUserManager.Setup(u => u.Users).Returns(users);

            mockPasswordHasher = new Mock<IPasswordHasher<ApplicationUser>>();

            adminController = new AdminController(mockUserManager.Object, mockPasswordHasher.Object);
        }

        public async Task Create_WithValidModelState_ReturnsViewResult()
        {
            // Arrange
            var user = new User
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Password = "password"
            };
            mockUserManager.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
            mockUserManager.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await adminController.Create(user) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Thêm mới thông tin tài khoản thành công !", adminController.TempData["ResultOk"]);
        }

        [Test]
        public async Task Create_WithInvalidModelState_ReturnsViewResult()
        {
            // Arrange
            adminController.ModelState.AddModelError("Email", "Email is required");
            var user = new User();

            // Act
            var result = await adminController.Create(user) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(adminController.ModelState.IsValid);
        }

        public async Task Update_WithValidModelState_ReturnsViewResult()
        {
            // Arrange
            var userEdit = new ApplicationUser
            {
                Id = "1",
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890"
            };
            mockUserManager.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userEdit);
            mockUserManager.Setup(u => u.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            var password = "password";

            // Act
            var result = await adminController.Update(userEdit, password) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Cập nhật thông tin tài khoản thành công !", adminController.TempData["ResultOk"]);
        }

        [Test]
        public async Task Update_WithInvalidModelState_ReturnsViewResult()
        {
            // Arrange
            adminController.ModelState.AddModelError("Email", "Email is required");
            var userEdit = new ApplicationUser();

            // Act
            var result = await adminController.Update(userEdit, null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(adminController.ModelState.IsValid);
        }

        public async Task Delete_WithValidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var user = new ApplicationUser { Id = "1" };
            mockUserManager.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            mockUserManager.Setup(u => u.DeleteAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            // Act
            var result = await adminController.Delete("1") as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Xoá tài khoản thành công !", adminController.TempData["ResultOk"]);
        }

        [Test]
        public async Task Delete_WithInvalidId_ReturnsViewResult()
        {
            // Arrange
            mockUserManager.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await adminController.Delete("1") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("User Not Found", adminController.ModelState[""].Errors[0].ErrorMessage);
        }
    }
}