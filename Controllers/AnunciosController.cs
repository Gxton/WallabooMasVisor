using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wallaboo.Data;
using Wallaboo.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Wallaboo.Controllers
{
    public class AnunciosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnunciosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Anuncios
        public async Task<IActionResult> Index()
        {
            //ACA HAY QUE PONER LA CONSULTA PARA TRAER SOLO LOS QUE NO EXTAN VENCIDOS
            return View(await _context.Anuncios.ToListAsync());
        }

        // GET: Anuncios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anuncio = await _context.Anuncios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anuncio == null)
            {
                return NotFound();
            }

            return View(anuncio);
        }

        // GET: Anuncios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Anuncios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,TenantId,FechaDesde,FechaHasta,Precio,CantidadDias,Activo,Pagado")] Anuncio anuncio)
        {
            int result = DateTime.Compare(anuncio.FechaDesde, anuncio.FechaHasta);


            if ((result <= 0) && (anuncio.FechaDesde <= DateTime.Now))
            { 
                DateTime fechad = Convert.ToDateTime(anuncio.FechaDesde);
                DateTime fechah = Convert.ToDateTime(anuncio.FechaHasta);
                TimeSpan diff = fechah - fechad;
                int dias = (int)diff.Days;
                if (dias < 1)
                {
                    dias = 1;
                }
                anuncio.CantidadDias = dias;
                anuncio.Activo = 1;
                anuncio.Pagado = 0;
                _context.Add(anuncio);
                await _context.SaveChangesAsync();                
            }
            else
            {
                ViewBag.Msg = "PRUEBA DE MENSAJEl";
            }
            return RedirectToAction(nameof(Index));


            //    if (ModelState.IsValid)
            //    {

            //}
            //return View(anuncio);
        }

        // GET: Anuncios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anuncio = await _context.Anuncios.FindAsync(id);
            if (anuncio == null)
            {
                return NotFound();
            }
            return View(anuncio);
        }

        // POST: Anuncios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,TenantId,FechaDesde,FechaHasta,Precio,CantidadDias,Activo,Pagado")] Anuncio anuncio)
        {
            if (id != anuncio.Id)
            {
                return NotFound();
            }
            int result = DateTime.Compare(anuncio.FechaDesde, anuncio.FechaHasta);

            //if (ModelState.IsValid)
            //{
            try
                {
                if ((result <= 0) && (anuncio.FechaDesde <= DateTime.Now))
                { 
                    DateTime fechad = Convert.ToDateTime(anuncio.FechaDesde);
                    DateTime fechah = Convert.ToDateTime(anuncio.FechaHasta);
                    TimeSpan diff = fechah - fechad;
                    int dias = (int)diff.Days;
                    if (dias < 1)
                    {
                        dias = 1;
                    }
                    anuncio.CantidadDias = dias;
                    _context.Update(anuncio);
                        await _context.SaveChangesAsync();
                }
                else { ViewBag.Msg = "PRUEBA DE MENSAJEl"; }
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnuncioExists(anuncio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            //return View(anuncio);
        }

        // GET: Anuncios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anuncio = await _context.Anuncios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anuncio == null)
            {
                return NotFound();
            }

            return View(anuncio);
        }

        // POST: Anuncios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var anuncio = await _context.Anuncios.FindAsync(id);
            if (anuncio != null)
            {
                _context.Anuncios.Remove(anuncio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnuncioExists(int id)
        {
            return _context.Anuncios.Any(e => e.Id == id);
        }
    }
}
