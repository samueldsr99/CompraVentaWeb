using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CompraVenta.Models;
using CompraVenta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace CompraVenta.Controllers
{
    [Authorize(Roles = "Client,Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<Account> userManager;
        private readonly SignInManager<Account> signInManager;
        private readonly AppDbContext context;
        private readonly IHostingEnvironment hostingEnvironment;

        public AccountController(UserManager<Account> userManager,
                                SignInManager<Account> signInManager,
                                IHostingEnvironment hostingEnvironment,
                                AppDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userByEmail = await userManager.FindByEmailAsync(model.Email);
                if (userByEmail == null)
                {
                    var user = new Account
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Description = model.Details,
                    };
                    var result = await userManager.CreateAsync(user, model.Password);
                    await userManager.AddToRoleAsync(user, "Client");

                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: true);
                        
                        context.SaveChanges();

                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"El correo {model.Email} ya existe.");
                }
            }
            
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<Account> hasher = new PasswordHasher<Account>();
                var hash = hasher.HashPassword(null, model.Password);
                
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
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
        
        [HttpGet]
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
                ProfileImagePath = account.ProfileImagePath,
                OldImagePath = account.ProfileImagePath
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = await userManager.FindByNameAsync(model.UserName);

                if (User.IsInRole("Admin") && model.Role != null)
                {
                    string oldRole = model.Role == "Admin" ? "Client" : "Admin";
                    await userManager.RemoveFromRoleAsync(account, oldRole);
                    await userManager.AddToRoleAsync(account, model.Role);
                }

                account.Email = model.Email;
                account.Name = model.Name;
                account.PhoneNumber = model.PhoneNumber;
                account.Description = model.Details;

                string uniqueFileName = null;
                if (model.ProfileImage != null)
                {
                    if (model.OldImagePath != null)
                    {
                        Utils.FileProcess.DeleteFile(model.OldImagePath, hostingEnvironment);
                    }

                    uniqueFileName = Utils.FileProcess.UploadFile(model.ProfileImage, hostingEnvironment);
                }
                account.ProfileImagePath = uniqueFileName;

                var result = await userManager.UpdateAsync(account);

                if (result.Succeeded)
                {
                    if (model.NewPassword != null
                    || model.OldPassword != null
                    || model.Confirmation != null)
                    {
                        var change_result = await userManager.ChangePasswordAsync(account, model.OldPassword, model.NewPassword);
                        
                        if (!change_result.Succeeded)
                        {
                            foreach (var error in change_result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            return View(model);
                        }
                    }
                    return RedirectToAction("AccountDetails", new { userName = User.Identity.Name });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return RedirectToAction("Edit", new { username = User.Identity.Name });
        }

        [HttpGet][HttpPost]
        [AllowAnonymous]
        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewBag.returnUrl = ReturnUrl;
            return View();
        }
    }
}
