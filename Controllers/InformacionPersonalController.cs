using ManejoPresupuestos.Models;
using ManejoPresupuestos.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ManejoPresupuestos.Controllers
{
    public class InformacionPersonalController : Controller
    {
        private readonly IRepositorioInformacionPersonal repositorioInformacionPersonal;
        private readonly IServicioUsuarios servicioUsuarios;

        public InformacionPersonalController(IRepositorioInformacionPersonal repositorioInformacionPersonal,
           IServicioUsuarios servicioUsuarios)
        {
            this.repositorioInformacionPersonal = repositorioInformacionPersonal;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index(string searchCedula)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            IEnumerable<InformacionPersonal> informacionPersonal;

            if (!string.IsNullOrEmpty(searchCedula))
            {
                ViewData["CurrentFilter"] = searchCedula;
                informacionPersonal = await repositorioInformacionPersonal.BuscarPorCedula(usuarioId, searchCedula);
            }
            else
            {
                informacionPersonal = await repositorioInformacionPersonal.Obtener(usuarioId);
            }

            return View(informacionPersonal);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(InformacionPersonal informacionPersonal)
        {
            if (!ModelState.IsValid)
            {
                return View(informacionPersonal);
            }
            informacionPersonal.UsuarioId = servicioUsuarios.ObtenerUsuarioId();
            await repositorioInformacionPersonal.Crear(informacionPersonal);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var informacionPersonal = await repositorioInformacionPersonal.ObtenerPorId(id, usuarioId);
            if (informacionPersonal is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(informacionPersonal);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(InformacionPersonal informacionPersonal)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var informacionPersonalExiste = await repositorioInformacionPersonal.ObtenerPorId(informacionPersonal.Id, usuarioId);
            if (informacionPersonalExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioInformacionPersonal.Actualizar(informacionPersonal);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var informacionPersonal = await repositorioInformacionPersonal.ObtenerPorId(id, usuarioId);
            if (informacionPersonal is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(informacionPersonal);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarInformacionPersonal(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var informacionPersonal = await repositorioInformacionPersonal.ObtenerPorId(id, usuarioId);
            if (informacionPersonal is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioInformacionPersonal.Borrar(id);
            return RedirectToAction("Index");
        }
    }
}

