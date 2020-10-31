using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Models
{
    public class IdentitySeedDB
    {
        public static void SeedDB(IServiceProvider serviceProvider)
        {
            CreateAdminAccountAsync(serviceProvider).Wait();
            CreateClientsAsync(serviceProvider).Wait();
        }

        private static async Task CreateAdminAccountAsync(IServiceProvider serviceProvider)
        {
            serviceProvider = serviceProvider.CreateScope().ServiceProvider;

            UserManager<Account> userManager = serviceProvider.GetRequiredService<UserManager<Account>>();

            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = "admin";
            string email = "admin@domain.com";

            string password = "admin";
            string role = "Admin";

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                Account user = new Account
                {
                    UserName = username,
                    Email = email
                };

                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }

        private static async Task CreateClientsAsync(IServiceProvider serviceProvider)
        {
            serviceProvider = serviceProvider.CreateScope().ServiceProvider;

            UserManager<Account> userManager = serviceProvider.GetRequiredService<UserManager<Account>>();

            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (await roleManager.FindByNameAsync("Client") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Client"));
            }


            var clients = new List<Tuple<Account, string>>
            {
                new Tuple<Account, string>(new Account
                {
                    UserName = "emilio",
                    Name = "Emilio Lopez Arteaga",
                    Email = "emilio69@gmail.com",
                    Description = "Empresario empedernido de Chile."
                }, "emilio123"),
                new Tuple<Account, string>(new Account
                {
                    UserName = "ricardo",
                    Name = "Ricardo Gutierrez Montes de Oca",
                    Email = "ricardo@gmail.com",
                    Description = "Me gusta el agucate."

                }, "ricardo123"),
                new Tuple<Account, string>(new Account
                {
                    UserName = "markzuckerberg",
                    Name = "Mark Zuckerberg",
                    Email = "mark@fbmail.com",
                    Description = "CEO of Facebook. Me sobra el dinero."
                }, "mark123"),
                new Tuple<Account, string>(new Account
                {
                    UserName = "samueldsr",
                    Name = "Samuel David Suarez Rodriguez",
                    Email = "samueldsr8@gmail.com",
                    Description = "CS student."
                }, "samueldsr"),
                new Tuple<Account, string>(new Account
                {
                    UserName = "masin",
                    Name = "Miguel Asin Barthelemy",
                    Email = "masin@gmail.com",
                    Description = "CS student"
                }, "masin123"),
                new Tuple<Account, string>(new Account
                {
                    UserName = "everdesia",
                    Name = "Enmanuel Verdesia Suarez",
                    Email = "everdesia@gmail.com",
                    Description = "CS student",
                }, "everdesia123"),
                new Tuple<Account, string>(new Account
                {
                    UserName = "magalis",
                    Name = "Magalis Ursula Consuegra",
                    Email = "magalis@gmail.com",
                    Description = "Vendo ropa caballero"
                }, "magalis123")
            };

            foreach (var userPassword in clients)
            {
                var user = userPassword.Item1;
                var password = userPassword.Item2;

                if (await userManager.FindByNameAsync(user.UserName) == null)
                {
                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Client");
                    }
                }
            }
        }
    }
}
