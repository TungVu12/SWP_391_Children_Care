using ASP_NET_MVC_Ver1.Areas.Identity.Data;
using ASP_NET_MVC_Ver1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_MVC_Ver1.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IPasswordHasher<ApplicationUser> passwordHasher;
        public AdminController(UserManager<ApplicationUser> usrMgr, IPasswordHasher<ApplicationUser> passwordHash)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            return View(userManager.Users);
        }
        [Authorize(Roles = "Admin")]
        public ViewResult Create() => View();
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                var defaultUser = CreateUser();

                defaultUser.Email = user.Email;
                defaultUser.LastName = user.FirstName;
                defaultUser.FirstName = user.LastName;
                defaultUser.PhoneNumber = user.PhoneNumber;
                defaultUser.EmailConfirmed = true;
                defaultUser.PhoneNumberConfirmed = true;
                defaultUser.ProfilePicture = new byte[0];

                if (userManager.Users.All(u => u.Id != defaultUser.Id))
                {
                    var userEntity = await userManager.FindByEmailAsync(defaultUser.Email);
                    if (userEntity == null)
                    {

                        await userManager.SetUserNameAsync(defaultUser, defaultUser.Email);
                        await userManager.SetEmailAsync(defaultUser, defaultUser.Email);

                       var result = await userManager.CreateAsync(defaultUser, user.Password);
                    if (result.Succeeded)
                    {
                            TempData["ResultOk"] = "Thêm mới thông tin tài khoản thành công !";
                            return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                            ModelState.AddModelError("", error.Description);
                    }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email đã được đăng ký");
                    }
                }
            }
                return View(user);

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(ApplicationUser userEdit,string password)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userEdit.Id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.Email))
                    user.Email = user.Email;
                else
                    ModelState.AddModelError("", "Email không được để trống");

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                else
                    ModelState.AddModelError("", "Password không được để trống");

                if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(password))
                {
                    user.PhoneNumber = userEdit.PhoneNumber;
                    user.FirstName = userEdit.FirstName;
                    user.LastName = userEdit.LastName;
                    user.ProfilePicture = new byte[0]; 
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["ResultOk"] = "Cập nhật thông tin tài khoản thành công !";
                        return RedirectToAction("Index");
                    }
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "Không tìm thấy tài khoản");
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["ResultOk"] = "Xoá tài khoản thành công !";
                    return RedirectToAction("Index");
                }
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
        private static ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
