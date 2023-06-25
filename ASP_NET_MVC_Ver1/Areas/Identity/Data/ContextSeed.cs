using ASP_NET_MVC_Ver1.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System;

namespace ASP_NET_MVC_Ver1.Areas.Identity.Data
{
    public static class ContextSeed
    {
        private static readonly IUserStore<ApplicationUser> _userStore;
        private static readonly IUserEmailStore<ApplicationUser> _emailStore;

        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Doctor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Nurse.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Parent.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Children.ToString()));
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


        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            //var defaultUser = new ApplicationUser
            //{
            //    Email = "superadmin@gmail.com",
            //    FirstName = "Super",
            //    LastName = "Admin",
            //    EmailConfirmed = true,
            //    PhoneNumberConfirmed = true
            //};

            var defaultUser = CreateUser();

            defaultUser.Email = "admin@gmail.com";
            defaultUser.LastName = "Admin";
            defaultUser.FirstName = "Manage";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;
            defaultUser.ProfilePicture = new byte[0];

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                   

                    await userManager.SetUserNameAsync(defaultUser, defaultUser.Email);
                    await userManager.SetEmailAsync(defaultUser, defaultUser.Email);

                    var result = await userManager.CreateAsync(defaultUser, "c");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Manager.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Doctor.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Nurse.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Parent.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Children.ToString());

                }

            }
        }

        public static async Task MyMethod(IUserStore<ApplicationUser> userStore, IUserEmailStore<ApplicationUser> emailStore)
        {
            var user = new ApplicationUser(); // Khởi tạo đối tượng user
            var email = "admin@gmail.com"; // Chuỗi email

            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        }
    }
}
