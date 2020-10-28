using CompraVenta.Models;
using CompraVenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Account> userManager;
        private readonly SignInManager<Account> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDbContext context;

        public AdminController(UserManager<Account> userManager,
                               SignInManager<Account> signInManager,
                               RoleManager<IdentityRole> roleManager,
                               AppDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.context = context;
        }
        
        // This endpoint has to be removed in the future
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAdminUser()
        {
            var admin = context.Users.FirstOrDefault(e => e.UserName.Equals("admin"));
            if (admin == null)
            {
                admin = new Account
                {
                    UserName = "admin"
                };

                await userManager.CreateAsync(admin, "admin");
                IdentityRole adminRole = new IdentityRole
                {
                    Name = "Admin"
                };
                IdentityRole clientRole = new IdentityRole
                {
                    Name = "Client"
                };

                if (await roleManager.FindByNameAsync(adminRole.Name) == null)
                {
                    await roleManager.CreateAsync(adminRole);
                }
                if (await roleManager.FindByNameAsync(clientRole.Name) == null)
                {
                    await roleManager.CreateAsync(clientRole);
                }

                // Assign admin role to admin user
                await userManager.AddToRoleAsync(admin, "Admin");

                return Json(true);
            }
            return Json("La cuenta admin ya ha sido creada.");
        }
        
        [HttpGet]
        public IActionResult ExampleAdminEndpoint()
        {
            return Json("Welcome admin.");
        }

        [HttpGet]
        public async Task<IActionResult> ListUsers()
        {
            var users = userManager.Users.ToList();
            List<UserViewModel> usersToShow = new List<UserViewModel>();

            foreach (var user in users)
            {
                string role = await userManager.IsInRoleAsync(user, "Admin") ? "Admin" : "Client";
                usersToShow.Add(new UserViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Role = role
                });
            }

            return View(new ListUsersViewModel
            {
                Users = usersToShow
            });
        }

        [HttpPost]
        public async Task<IActionResult> MakeUserAdmin(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return View("NotFound");
            }

            if (await userManager.IsInRoleAsync(user, "Admin"))
            {
                await userManager.RemoveFromRoleAsync(user, "Admin");
                await userManager.AddToRoleAsync(user, "Client");
            }
            else
            {
                await userManager.RemoveFromRoleAsync(user, "Client");
                await userManager.AddToRoleAsync(user, "Admin");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return View("NotFound");
            }

            await userManager.DeleteAsync(user);

            return RedirectToAction("ListUsers", "Admin");
        }
    }
}
