using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wallabo.Entities;
using Wallaboo.Data;
using Wallaboo.Models;
using Wallaboo.Services;

namespace Wallaboo.Controllers
{
    public class UsuariosController: Controller
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
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
                    URLComercial = Constantes.UrlComercial + usuarioIdentity.Id
                };
                _context.Add(usuario);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(modelo);
            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
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
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto.");
                return View(modelo);
            }
        }
    }
}
