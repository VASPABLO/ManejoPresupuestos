using ManejoPresupuestos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ManejoPresupuestos.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public PrincipalController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        
    }
}