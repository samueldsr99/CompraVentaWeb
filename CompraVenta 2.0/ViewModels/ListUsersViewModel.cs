using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.ViewModels
{
    public class ListUsersViewModel
    {
        public List<UserViewModel> Users;
    }

    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
    }
}
