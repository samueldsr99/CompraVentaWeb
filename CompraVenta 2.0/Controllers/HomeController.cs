using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompraVenta.Models;
using Microsoft.AspNetCore.Authorization;

namespace CompraVenta.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Contact() => View();
        public IActionResult Services() => View();
        public IActionResult About() => View();
    }
}
