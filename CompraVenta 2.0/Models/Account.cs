using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Models
{
    public class Account : IdentityUser
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
