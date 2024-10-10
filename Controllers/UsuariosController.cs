using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Wallabo.Entities; // Asegúrate de incluir el espacio de nombres de tu modelo
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Security.Claims;
using Wallaboo.Data;
using Wallaboo.Models;
using Wallaboo.Services;
using Microsoft.IdentityModel.Tokens;


namespace Wallaboo.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public UsuariosController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._context = context;
        }

        public IActionResult Registro()
        {
            var paises = _context.Paises.ToList();
            var modelo = new RegistroViewModel();
            modelo.ListaPaises = paises;
            return View(modelo);

        }

        //COMIENZO PRUEBA COMBO
        [HttpGet]
        public JsonResult GetPaisDescripcion(int id)
        {
            var pais = _context.Paises.FirstOrDefault(x => x.id == id);
            // Lógica para obtener la descripción del país por el id
            return Json(new { descripcion = pais.NombrePais });
        }

        [HttpGet]
        public JsonResult GetProvinciaDescripcion(int id)
        {
            var provincia = _context.Provincias.FirstOrDefault(x => x.id == id);
            return Json(new { descripcion = provincia.NombreProvincia });
        }

        [HttpGet]
        public JsonResult GetCiudadDescripcion(int id)
        {
            var ciudad = _context.Ciudades.FirstOrDefault(x => x.Id == id);
            return Json(new { descripcion = ciudad.NombreCiudad });
        }

        [HttpGet]
        public IActionResult GetStates(int countryId)
        {
            var states = _context.Provincias.Where(x => x.PaisId == countryId).ToList();
            return Json(new SelectList(states, "id", "NombreProvincia"));
        }
        [HttpGet]
        public IActionResult GetCities(int stateId)
        {
            var cities = _context.Ciudades.Where(x => x.ProvinciaId == stateId).ToList();
            return Json(new SelectList(cities, "ProvinciaId", "NombreCiudad"));
        }
        //FIN DE PREUBA COMBO

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
        public async Task<IActionResult> Detalle()
        {
            if (_userManager.GetUserId(User) == null)
            {
                return NotFound();
            }

            var usr = await _context.Usuarios.FindAsync(_userManager.GetUserId(User));
            if (_userManager.GetUserId(User) == null)
            {
                return NotFound();
            }
            return View(usr);
        }


        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel modelo, int _pais, int _provincia, int _ciudad)
        {
            modelo.PaisId = 1;
            //modelo.PaisId = _pais;
            modelo.ProvinciaId = _provincia;
            modelo.CiudadId = _ciudad;
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }


            var usuarioIdentity = new IdentityUser() { Email = modelo.Email, UserName = modelo.Email };

            var resultado = await _userManager.CreateAsync(usuarioIdentity, password: modelo.Password);

            var claimsPersonalizados = new List<Claim>()
            {
                new Claim(Constantes.ClaimTenantId, usuarioIdentity.Id),
            };

            await _userManager.AddClaimsAsync(usuarioIdentity, claimsPersonalizados);

            if (resultado.Succeeded)
            {
                await _signInManager.SignInAsync(usuarioIdentity, isPersistent: true);

                var usuario = new Usuario()
                {
                    TenantId = usuarioIdentity.Id,
                    NombreComercial = modelo.NombreComercial,
                    DireccionComercial = modelo.DireccionComercial,
                    TelefonoComercial = modelo.TelefonoComercial,
                    DescripcionComercial = modelo.DescripcionComercial,
                    URLComercial = Constantes.UrlComercial + usuarioIdentity.Id,
                    HorarioComercial = modelo.HorarioComercial,
                    PaisId = modelo.PaisId,
                    ProvinciaId = modelo.ProvinciaId,
                    CiudadId = modelo.CiudadId
                };
                // Generar el código QR
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(usuario.URLComercial, QRCodeGenerator.ECCLevel.Q);

                // Generar el gráfico en formato PNG (Bitmap no se requiere directamente aquí)
                PngByteQRCode pngQRCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeBytes = pngQRCode.GetGraphic(20);

                // Guardar el código QR en la base de datos

                usuario.QRURL = qrCodeBytes;

                _context.Add(usuario);
                _context.SaveChanges();

                return RedirectToAction("Index", "Anuncios");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                //return View(modelo);
                return RedirectToAction("Error", "Usuarios");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await _signInManager.PasswordSignInAsync(modelo.Email,
                   modelo.Password, modelo.RememberMe, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                //ViewBag.tt = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "Anuncios");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto.");
                return View(modelo);
            }
        }
        public async Task<IActionResult> EditUsr()
        {
            if (_userManager.GetUserId(User) == null)
            {
                return NotFound();
            }

            var usr = await _context.Usuarios.FindAsync(_userManager.GetUserId(User));
            if (_userManager.GetUserId(User) == null)
            {
                return NotFound();
            }
            return View(usr);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsr([Bind("NombreComercial, DireccionComercial, TelefonoComercial, URLComercial, DescripcionComercial,HorarioComercial, TenantId, PaisId, ProvinciaId, CiudadId")] Usuario usuario)
        {
            if (_userManager.GetUserId(User) != usuario.TenantId)
            {
                return NotFound();
            }
            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.TenantId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index), "anuncios");
        }
        private bool UsuarioExists(string tenantId)
        {
            return _context.Usuarios.Any(e => e.TenantId == tenantId);
        }


        public IActionResult GeneratePdf(string id)
        {
            var usuario = _context.Usuarios.Find(id);

            if (usuario == null)
            {
                return NotFound();
            }

            try
            {
                // Crear el documento PDF
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Nuestras ofertas!";

                // Crear una página
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Configurar fuentes
                XFont titleFont = new XFont("Arial", 45, XFontStyle.Bold);
                XFont textFont = new XFont("Arial", 28, XFontStyle.Regular);
                XFont footerFont = new XFont("Arial", 12, XFontStyle.Regular);

                // Márgenes
                double margin = 40;
                double contentWidth = page.Width - margin * 2;
                double yPosition = margin;

                // Dibuja el marco alrededor de todo el contenido del PDF
                XPen borderPen = new XPen(XColors.Black, 2);
                gfx.DrawRectangle(borderPen, margin / 2, margin / 2, page.Width - margin, page.Height - margin);

                // Título
                gfx.DrawString(usuario.NombreComercial ?? "N/A", titleFont, XBrushes.Black,
                    new XRect(margin, yPosition, contentWidth, 40), XStringFormats.Center);
                yPosition += 50;

                gfx.DrawString("Nuestras ofertas de hoy", textFont, XBrushes.Black,
                    new XRect(margin, yPosition, contentWidth, 40), XStringFormats.Center);

                // Espacio antes del código QR
                yPosition += 50;

                // Dibuja una línea que atraviese toda la página antes del QR
                yPosition += 20;
                gfx.DrawLine(XPens.Black, margin, yPosition, page.Width - margin, yPosition);
                yPosition += 30;

                // Generar el código QR solo si existe el QRURL y no es vacío
                if (usuario.QRURL != null && usuario.QRURL.Length > 0)
                {
                    // Convertir el byte[] a una imagen
                    using (var ms = new MemoryStream(usuario.QRURL))
                    {
                        using (var qrImage = System.Drawing.Image.FromStream(ms))
                        {
                            using (MemoryStream imageStream = new MemoryStream())
                            {
                                qrImage.Save(imageStream, System.Drawing.Imaging.ImageFormat.Png);
                                imageStream.Position = 0; // Reiniciar el stream

                                XImage xImage = XImage.FromStream(() => imageStream);
                                double qrSize = 300; // Ajustar el tamaño del código QR
                                gfx.DrawImage(xImage, (page.Width - qrSize) / 2, yPosition, qrSize, qrSize);
                            }
                        }
                    }
                }

                // Pie de página
                gfx.DrawLine(XPens.Black, margin, page.Height - 50, page.Width - margin, page.Height - 50);
                gfx.DrawString("www.wallaboo.com - Marketing de Ofertas", footerFont, XBrushes.Gray,
                    new XRect(margin, page.Height - 40, contentWidth, 20), XStringFormats.Center);

                // Guardar el documento en un MemoryStream
                using (var memoryStream = new MemoryStream())
                {
                    document.Save(memoryStream, false);
                    var fileBytes = memoryStream.ToArray();
                    var fileName = $"{usuario.NombreComercial}.pdf".Replace(" ", "_");

                    return File(fileBytes, "application/pdf", fileName);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generando el PDF: {ex.Message}");
            }
        }






    }
}


