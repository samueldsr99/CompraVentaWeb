using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Models
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public SQLUserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Account Add(Account user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public Account Delete(int id)
        {
            Account user = context.Users.Find(id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
            return user;
        }

        public Account GetUser(int id)
        {
            return context.Users.Find(id);
        }

        public IEnumerable<Account> GetUsers()
        {
            return context.Users;
        }

        public Account Update(Account user)
        {
            var target = context.Users.Attach(user);
            target.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return user;
        }
    }
}
