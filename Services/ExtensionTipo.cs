using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using Wallaboo.Interfaces;

namespace Wallaboo.Services
{
    public static class ExtensionTipo
    {
        public static bool DebeSaltarValidacionTenant(this Type t)
        {
            var booleanos = new List<bool>()
            {
                t.IsAssignableFrom(typeof(IdentityRole)),
                t.IsAssignableFrom(typeof(IdentityRoleClaim<string>)),
                t.IsAssignableFrom(typeof(IdentityUser)),
                t.IsAssignableFrom(typeof(IdentityUserLogin<string>)),
                t.IsAssignableFrom(typeof(IdentityUserRole<string>)),
                t.IsAssignableFrom(typeof(IdentityUserToken<string>)),
                t.IsAssignableFrom(typeof(IdentityUserClaim<string>)),
                typeof(IEntidadComun).IsAssignableFrom(t)
            };
            var resultado = booleanos.Aggregate((b1, b2) => b1 || b2);
            return resultado;
        }
    }
}
