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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using Microsoft.AspNetCore.Http;
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
            // Cargar todos los anuncios con sus imágenes asociadas
            var anunciosConImagenes = await _context.Anuncios
                .Include(a => a.Imagenes) // Asegurarse de incluir las imágenes
                .OrderBy(a => a.FechaHasta)
                .ToListAsync();

            // Validar las rutas de las imágenes y asegurarse de que existan en el sistema de archivos
            foreach (var anuncio in anunciosConImagenes)
            {
                if (anuncio.Imagenes != null && anuncio.Imagenes.Any())
                {
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", anuncio.TenantId);

                    foreach (var imagen in anuncio.Imagenes)
                    {
                        // Verifica si la imagen tiene una ruta y si existe en el sistema de archivos
                        var imagenPath = Path.Combine(uploadPath, Path.GetFileName(imagen.Image1Path));
                        if (!System.IO.File.Exists(imagenPath))
                        {
                            // Si no existe la imagen, podrías establecer una imagen por defecto o manejar el error
                            imagen.Image1Path = "/uploads/default-image.png"; // Imagen por defecto
                        }
                    }
                }
            }

            return View(anunciosConImagenes);
        }


        // GET: Anuncios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obtén el anuncio junto con sus imágenes
            var anuncio = await _context.Anuncios
                .Include(a => a.Imagenes) // Asegúrate de incluir las imágenes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (anuncio == null)
            {
                return NotFound();
            }

            // Crea el modelo para la vista de detalles
            var model = new HomeIndexViewModel
            {
                // Asigna las propiedades necesarias
                ID = anuncio.Id,
                Descripcion = anuncio.Descripcion,
                FechaDesde = anuncio.FechaDesde,
                FechaHasta = anuncio.FechaHasta,
                Precio = anuncio.Precio,
                CantidadDias = anuncio.CantidadDias,
                Activo = anuncio.Activo,
                Pagado = anuncio.Pagado,
                ImagenesGuardadas = anuncio.Imagenes // Asigna las imágenes directamente
            };

            // Crear una lista de URLs de imágenes
            var imagenUrls = anuncio.Imagenes
                .Where(imagen => !string.IsNullOrEmpty(imagen.Image1Path) && !string.IsNullOrEmpty(imagen.TenantId)) // Filtra imágenes válidas
                .Select(imagen =>
                    Url.Content($"~/uploads/{imagen.TenantId}/{System.IO.Path.GetFileName(imagen.Image1Path)}"))
                .ToList();

            // Asigna las URLs a una nueva propiedad en el modelo de vista
            ViewBag.ImagenUrls = imagenUrls;

            return View(model); // Pasa el modelo a la vista
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

                // Guardar el anuncio antes de las imágenes
                _context.Add(anuncio);
                await _context.SaveChangesAsync(); // Guarda el anuncio primero para obtener el Id

                // Manejo de imágenes
                if (imagenes != null && imagenes.Count > 0)
                {
                    // Crear la ruta de carga basada en el TenantId
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", anuncio.TenantId);

                    // Crear el directorio si no existe
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    foreach (var file in imagenes)
                    {
                        if (file.Length > 0)
                        {
                            // Generar un nombre único para el archivo
                            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                            var filePath = Path.Combine(uploadPath, fileName);

                            // Redimensionar y recortar la imagen a cuadrada
                            using (var image = Image.Load(file.OpenReadStream()))
                            {
                                // Calcular el tamaño del nuevo lado
                                var size = Math.Max(image.Width, image.Height);

                                // Redimensionar y recortar la imagen
                                image.Mutate(x => x
                                    .Resize(new ResizeOptions
                                    {
                                        Size = new Size(size, size),
                                        Mode = ResizeMode.Crop
                                    }));

                                // Guarda la imagen en la ruta especificada
                                await image.SaveAsync(filePath); // Guardar imagen procesada
                            }

                            // Crea la entidad Imagen con la ruta relativa correcta
                            var imagen = new Imagen
                            {
                                AnuncioId = anuncio.Id,
                                TenantId = anuncio.TenantId,
                                Image1Path = Path.Combine("uploads", anuncio.TenantId, fileName) // Guarda la ruta relativa
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

            // Cargar el anuncio y sus imágenes asociadas
            var anuncio = await _context.Anuncios
                .Include(a => a.Imagenes) // Incluye la colección de imágenes
                .FirstOrDefaultAsync(a => a.Id == id);

            if (anuncio == null)
            {
                return NotFound();
            }

            // Inicializa la colección de imágenes si es nula
            anuncio.Imagenes ??= new List<Imagen>();

            // Obtener la fecha y hora actual del servidor
            ViewBag.CurrentDateTime = DateTime.Now;

            return View(anuncio);
        }


        // POST: Anuncios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,TenantId,FechaDesde,FechaHasta,Precio,CantidadDias,Activo,Pagado")] Anuncio anuncio, IFormFileCollection imagenes)
        {
            if (id != anuncio.Id)
            {
                return NotFound();
            }

            // Validación de las fechas
            int result = DateTime.Compare(anuncio.FechaDesde, anuncio.FechaHasta);

            if ((result <= 0) && (anuncio.FechaDesde >= DateTime.Today))
            {
                // Lógica para calcular días y asignar propiedades
                DateTime fechad = Convert.ToDateTime(anuncio.FechaDesde);
                DateTime fechah = Convert.ToDateTime(anuncio.FechaHasta);
                TimeSpan diff = fechah - fechad;
                int dias = (int)diff.Days < 1 ? 1 : (int)diff.Days;
                anuncio.CantidadDias = dias;

                // Actualiza el anuncio
                _context.Update(anuncio);

                // Manejo de imágenes
                if (imagenes != null && imagenes.Count > 0)
                {
                    // Crear la ruta de carga basada en el TenantId
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", anuncio.TenantId);

                    // Crear el directorio si no existe
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Borra imágenes existentes asociadas al anuncio (opcional)
                    var existingImages = await _context.Imagenes.Where(i => i.AnuncioId == anuncio.Id).ToListAsync();
                    if (existingImages.Any())
                    {
                        // Elimina las imágenes del sistema de archivos
                        foreach (var img in existingImages)
                        {
                            if (!string.IsNullOrEmpty(img.Image1Path)) // Verifica que la ruta no sea nula
                            {
                                var imgPath = Path.Combine(uploadPath, Path.GetFileName(img.Image1Path));
                                if (System.IO.File.Exists(imgPath))
                                {
                                    System.IO.File.Delete(imgPath);
                                }
                            }
                        }
                        _context.Imagenes.RemoveRange(existingImages);
                    }

                    // Agrega las nuevas imágenes
                    foreach (var file in imagenes)
                    {
                        if (file.Length > 0)
                        {
                            // Usar el nombre original del archivo
                            var originalFileName = Path.GetFileName(file.FileName);
                            var filePath = Path.Combine(uploadPath, originalFileName); // Ruta con el nombre original

                            // Redimensionar y recortar la imagen a cuadrada
                            using (var image = Image.Load(file.OpenReadStream()))
                            {
                                // Calcular el tamaño del nuevo lado
                                var size = Math.Max(image.Width, image.Height);

                                // Redimensionar y recortar la imagen
                                image.Mutate(x => x
                                    .Resize(new ResizeOptions
                                    {
                                        Size = new Size(size, size),
                                        Mode = ResizeMode.Crop
                                    }));

                                // Guarda la imagen en la ruta especificada
                                await image.SaveAsync(filePath); // Guardar imagen procesada
                            }

                            // Crea la entidad Imagen
                            var imagen = new Imagen
                            {
                                AnuncioId = anuncio.Id,
                                TenantId = anuncio.TenantId,
                                Image1Path = Path.Combine("uploads", anuncio.TenantId, originalFileName) // Guarda la ruta relativa con el nombre original
                            };

                            _context.Imagenes.Add(imagen);
                        }
                    }
                }

                await _context.SaveChangesAsync(); // Guarda los cambios en el anuncio y las imágenes

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Si la validación falla, recargar las imágenes desde la base de datos
                var anuncioFromDb = await _context.Anuncios
                                                  .Include(a => a.Imagenes) // Incluir las imágenes asociadas
                                                  .FirstOrDefaultAsync(a => a.Id == anuncio.Id);

                if (anuncioFromDb != null)
                {
                    anuncio.Imagenes = anuncioFromDb.Imagenes; // Asignar las imágenes desde la base de datos
                }

                ViewBag.Msg = "La fecha de inicio y fin de la publicación deben ser posteriores a la fecha y hora actual.";
                return View(anuncio); // Devuelve el anuncio con las imágenes recargadas para que el usuario pueda corregirlo
            }
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
                // Elimina las imágenes asociadas al anuncio
                var imagenes = await _context.Imagenes.Where(i => i.AnuncioId == anuncio.Id).ToListAsync();
                if (imagenes.Any())
                {
                    var baseUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    var tenantUploadPath = Path.Combine(baseUploadPath, anuncio.TenantId);

                    // Elimina los archivos de imagen del sistema de archivos
                    foreach (var imagen in imagenes)
                    {
                        if (!string.IsNullOrEmpty(imagen.Image1Path)) // Verifica que Image1Path no sea nulo o vacío
                        {
                            var filePath = Path.Combine(tenantUploadPath, imagen.Image1Path);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                    }

                    // Elimina las imágenes de la base de datos
                    _context.Imagenes.RemoveRange(imagenes);
                }

                // Elimina el anuncio
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

            var cantidadDias = anuncio.CantidadDias;
            ViewBag.Texto = cantidadDias;
            //return View(anuncio);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        //PAra prubas locales de estado de pago
        public async Task<IActionResult> Pay(int id, [Bind("Id,Descripcion,TenantId,FechaDesde,FechaHasta,Precio,CantidadDias,Activo,Pagado")] Anuncio anuncio, Pago pago)
        {
            //var anuncioPagar = await _context.Anuncios.FindAsync(id);
            if (id != anuncio.Id)
            {
                return NotFound();
            }
            try
            {
                pago.total = ViewBag.Total;
                pago.AnuncioID = anuncio.Id;
                pago.TenantId = anuncio.TenantId;
                pago.FechaPago = ViewBag.FechaPago;
                _context.Add(pago);
                await _context.SaveChangesAsync();

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
    }

}
