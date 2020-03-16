using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompraVenta.Models;

namespace CompraVenta.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        public IActionResult Index() => View();
        public IActionResult Contact() => View();
        public IActionResult Services() => View();
        public IActionResult About() => View();
    }
}
