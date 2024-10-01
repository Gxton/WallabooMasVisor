using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wallaboo.Data;
using Wallaboo.Entities;
using Wallaboo.Interfaces;
using Wallaboo.Models;
using Wallaboo.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Wallaboo.Controllers
{
    public class AnunciosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMercadoPagoService _mercadoPagoService;
        public AnunciosController(ApplicationDbContext context, IMercadoPagoService mercadoPagoService)
        {
            _context = context;
            _mercadoPagoService = mercadoPagoService ?? throw new ArgumentNullException(nameof(mercadoPagoService));
        }

        // GET: Anuncios
        public async Task<IActionResult> Index()
        {
            //ver de eliminar desde la BD con un trigger cuando cumplen 15 dias de vencidas
            return View(await _context.Anuncios
                .Include(a => a.Imagenes)
                .OrderBy(a => a.FechaHasta).ToListAsync());
            //var fechita = DateTime.Now.AddDays(7);
            //return View(await _context.Anuncios.Where(a => a.FechaHasta <= fechita)
            //    .OrderBy(a => a.FechaHasta).ToListAsync());
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

        public async Task<IActionResult> Create([Bind("Id,Descripcion,TenantId,FechaDesde,FechaHasta,Precio,CantidadDias,Activo,Pagado")] Anuncio anuncio, IFormFileCollection imagenes)
        {
            int result = DateTime.Compare(anuncio.FechaDesde, anuncio.FechaHasta);

            if ((result <= 0) && (anuncio.FechaDesde >= DateTime.Today))
            {
                // Lógica para calcular días y asignar propiedades
                DateTime fechad = Convert.ToDateTime(anuncio.FechaDesde);
                DateTime fechah = Convert.ToDateTime(anuncio.FechaHasta);
                TimeSpan diff = fechah - fechad;
                int dias = (int)diff.Days < 1 ? 1 : (int)diff.Days;
                anuncio.CantidadDias = dias;
                anuncio.Activo = 0;
                anuncio.Pagado = 0;

                _context.Add(anuncio);
                await _context.SaveChangesAsync(); // Guarda el anuncio primero

                // Manejo de imágenes
                if (imagenes != null && imagenes.Count > 0)
                {
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    foreach (var file in imagenes)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                            var filePath = Path.Combine(uploadPath, fileName);

                            // Guarda el archivo
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            // Crea la entidad Imagen
                            var imagen = new Imagen
                            {
                                AnuncioId = anuncio.Id,
                                TenantId = anuncio.TenantId,
                                Image1Path = fileName // Solo guarda el nombre
                            };

                            _context.Imagenes.Add(imagen);
                        }
                    }
                    await _context.SaveChangesAsync(); // Guarda las imágenes
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Msg = "La fecha de inicio y fin de la publicación deben ser posteriores a la fecha y hora actual.";
                return View();
            }
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
                if ((result <= 0) && (anuncio.FechaDesde >= DateTime.Today))
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
                else
                {
                    ViewBag.Msg = "La fecha de inicio y fin de la publicacion deben ser posteriores a la fecha y hora actual";
                    return View();
                }
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
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Pay(int? id)
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
            var total = anuncio.CantidadDias * Constantes.ValorUnitario;
            ViewBag.Total = total;

            var texto = anuncio.Descripcion;
            ViewBag.Texto = texto;
            //return View(anuncio);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        //PAra prubas locales de estado de pago
        public async Task<IActionResult> Pay(int id, [Bind("Id,Descripcion,TenantId,FechaDesde,FechaHasta,Precio,CantidadDias,Activo,Pagado")] Anuncio anuncio)
        {
            //var anuncioPagar = await _context.Anuncios.FindAsync(id);
            if (id != anuncio.Id)
            {
                return NotFound();
            }
            try
            {
                anuncio.Pagado = 1;
                anuncio.Activo = 1;
                _context.Update(anuncio);
                await _context.SaveChangesAsync();
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
        }


        //[HttpGet("customerFind")]
        [HttpGet]
        public async Task<IActionResult> GetCustomerId([FromQuery] string email)
        {
            try
            {
                var customerId = await _mercadoPagoService.GetCustomerIdByEmailAsync(email);

                if (customerId != null)
                {
                    return Ok(customerId);
                }
                else
                {
                    return NotFound($"No se encontró el customer_id para el email: {email}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener el customer_id: {ex.Message}");
            }
        }
        //[HttpPost("create")]
        [HttpPost] //CreatePayment//
        public async Task<IActionResult> Pay([FromForm] decimal amount, [FromForm] string description, [FromForm] string cardNumber, [FromForm] int expirationMonth, [FromForm] int expirationYear, [FromForm] string cardholderName, [FromForm] string securityCode, [FromForm] string email,
                int id, Anuncio anuncio, Pago pago)
        {
            try
            {
                // Generar el cardToken
                var cardToken = await _mercadoPagoService.GenerateCardTokenAsync(cardNumber, expirationMonth, expirationYear, cardholderName, securityCode);

                // Obtener el customerId del email proporcionado
                var customerId = await _mercadoPagoService.GetCustomerIdByEmailAsync(email);

                if (customerId == null)
                {
                    // Si no se encuentra el customerId, crear un nuevo cliente en MercadoPago
                    customerId = await _mercadoPagoService.CreateCustomerAsync(email);
                }

                // Asociar la tarjeta al cliente (opcional, dependiendo de la lógica de tu aplicación)
                // var cardId = await _mercadoPagoService.CreateCardAsync(customerId, cardToken);

                // Crear el pago utilizando los datos del formulario y el cardToken generado
                var payment = await _mercadoPagoService.CreatePaymentAsync(amount, description, customerId, cardToken, securityCode, email);

                //Insertado mio
                if (id != anuncio.Id)
                {
                    return NotFound();
                }
                try
                {
                    //modifico el estado de activo y pagad del anuncio
                    anuncio.Pagado = 1;
                    anuncio.Activo = 1;
                    _context.Update(anuncio);
                    await _context.SaveChangesAsync();

                    //ingreso el pago en la tabla de pagos
                    pago.total = amount;
                    pago.AnuncioID = anuncio.Id;
                    pago.TenantId = anuncio.TenantId;
                    pago.FechaPago = DateTime.Now;

                    _context.Add(pago);
                    await _context.SaveChangesAsync();

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
                //fin insertado mio

                //return Ok(payment); //de MP original
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar el pago: {ex.Message}");
            }
        }
    }

}
