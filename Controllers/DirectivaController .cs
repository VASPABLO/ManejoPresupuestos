using ManejoPresupuestos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ManejoPresupuestos.Controllers
{
    public class DirectivaController : Controller
    {
        private readonly ILogger<HomeController>_logger;

        public DirectivaController(ILogger<HomeController> logger)
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