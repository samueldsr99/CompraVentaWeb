using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompraVenta.Models;
using CompraVenta.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompraVenta.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Account> userManager;
        private readonly SignInManager<Account> signInManager;
        private readonly AppDbContext context;

        public AccountController(UserManager<Account> userManager,
                                SignInManager<Account> signInManager, AppDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Account
                {
                    UserName = model.UserName,
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Description = model.Details,
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Login Fallido");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult AccountDetails(string userName)
        {
            var account = userManager.Users.FirstOrDefault(e => e.UserName.Equals(userName));
            return View(account);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string userName)
        {
            var account = await userManager.FindByNameAsync(userName);
            var model = new EditAccountViewModel
            {
                UserName = account.UserName,
                Name = account.Name,
                Email = account.Email,
                Details = account.Description,
                PhoneNumber = account.PhoneNumber,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = await userManager.FindByNameAsync(model.UserName);

                account.Email = model.Email;
                account.Name = model.Name;
                account.PhoneNumber = model.PhoneNumber;
                account.Description = model.Details;

                var result = await userManager.UpdateAsync(account);

                if (result.Succeeded)
                {
                    if (model.NewPassword != null
                    || model.OldPassword != null
                    || model.Confirmation != null)
                    {
                        var change_result = await userManager.ChangePasswordAsync(account, model.OldPassword, model.NewPassword);
                        if (change_result.Succeeded)
                        {
                            return View(model);
                        }
                        else
                        {
                            foreach (var error in change_result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            return View(model);
                        }
                    }
                    else
                    {
                        return View(model);
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}
