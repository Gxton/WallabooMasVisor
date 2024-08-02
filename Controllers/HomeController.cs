using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Wallaboo.Data;
using Wallaboo.Entities;
using Wallaboo.Models;

namespace Wallaboo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var modelo = await ConstruirModeloHomeIndex();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Anuncio anuncio)
        {
            context.Add(anuncio);
            await context.SaveChangesAsync();
            var modelo =await ConstruirModeloHomeIndex();
            return View(modelo);
        }

        private async Task<HomeIndexViewModel> ConstruirModeloHomeIndex()
        {
            var anuncio = await context.Anuncios.ToListAsync();
            var paises = await context.Paises.ToListAsync();

            var modelo = new HomeIndexViewModel();

            modelo.Anuncios = anuncio;
            modelo.Paises = paises;
            return modelo;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
