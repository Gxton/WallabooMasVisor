using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallaboo.Data;
using Wallaboo.Entities;
using Wallaboo.Models;

namespace Wallaboo.Controllers
{
    public class AnuncioController : Controller
    {
        private readonly ILogger<AnuncioController> _logger;
        private readonly ApplicationDbContext context;
        public AnuncioController(ILogger<AnuncioController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }
        public async Task<IActionResult> Carga()
        {
            var modelo = await ConstruirModeloAnuncio();
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Carga(Anuncio anuncio)
        {
            context.Add(anuncio);
            await context.SaveChangesAsync();
            var modelo = await ConstruirModeloAnuncio();
            return View(modelo);
        }

        private async Task<AnuncioViewModel> ConstruirModeloAnuncio()
        {
            var anuncio = await context.Anuncios.ToListAsync();
            //var paises = await context.Paises.ToListAsync();

            var modelo = new AnuncioViewModel();

            modelo.Anuncios = anuncio;
            //modelo.Paises = paises;
            return modelo;
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
