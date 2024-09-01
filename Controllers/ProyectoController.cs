using ManejoPresupuestos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ManejoPresupuestos.Controllers
{
    public class ProyectoController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ProyectoController(ILogger<HomeController> logger)
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