using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Models
{
    public interface IUserRepository
    {
        Account GetUser(int id);
        IEnumerable<Account> GetUsers();
        Account Add(Account user);
        Account Update(Account user);
        Account Delete(int id);
    }
}
