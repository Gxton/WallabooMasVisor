using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Wallabo.Entities;
using Wallaboo.Entities;
using Wallaboo.Entities.Configs;
using Wallaboo.Interfaces;
using Wallaboo.Services;


namespace Wallaboo.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
            private string tenantId;

            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                IServiceTenant servicioTenant)
                : base(options)
            {
                tenantId = servicioTenant.ObtenerTenant();
            }

            public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            {
                foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Added
                && e.Entity is IEntidadTenant))
                {
                    if (string.IsNullOrEmpty(tenantId))
                    {
                        throw new Exception("TenantId no encontrado al momento de crear el registro.");
                    }

                    var entidad = item.Entity as IEntidadTenant;
                    entidad!.TenantId = tenantId;
                }

                return base.SaveChangesAsync(cancellationToken);
            }


            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AnuncioConfig());
            builder.ApplyConfiguration(new CiudadConfig());
            builder.ApplyConfiguration(new PaisConfig());
            builder.ApplyConfiguration(new ProvinciaConfig());
            builder.ApplyConfiguration(new UsuarioConfig());

            //builder.Entity<Pais>().HasData(new Pais[]
            //    {
            //    new Pais{id = 1, NombrePais = "República Dominicana"},
            //    new Pais{id = 2, NombrePais = "México"},
            //    new Pais{id = 3, NombrePais = "Colombia"}
            //    });


            foreach (var entidad in builder.Model.GetEntityTypes())
                {
                    var tipo = entidad.ClrType;

                    if (typeof(IEntidadTenant).IsAssignableFrom(tipo))
                    {
                        var método = typeof(ApplicationDbContext)
                            .GetMethod(nameof(ArmarFiltroGlobalTenant),
                            BindingFlags.NonPublic | BindingFlags.Static
                               )?.MakeGenericMethod(tipo);

                        var filtro = método?.Invoke(null, new object[] { this })!;
                        entidad.SetQueryFilter((LambdaExpression)filtro);
                        entidad.AddIndex(entidad.FindProperty(nameof(IEntidadTenant.TenantId))!);
                    }
                    else if (tipo.DebeSaltarValidacionTenant())
                    {
                        continue;
                    }
                    else
                    {
                        throw new Exception($"La entidad {entidad} no ha sido marcada como tenant o común");
                    }
                }
            }

            private static LambdaExpression ArmarFiltroGlobalTenant<TEntidad>(
                ApplicationDbContext context)
                where TEntidad : class, IEntidadTenant
            {
                Expression<Func<TEntidad, bool>> filtro = x => x.TenantId == context.tenantId;
                return filtro;
            }

        public DbSet<Anuncio> Anuncios => Set<Anuncio>();
        public DbSet<Pais> Paises => Set<Pais>();
        public DbSet<Provincia> Provincias => Set<Provincia>();
        public DbSet<Ciudad> Ciudades => Set<Ciudad>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        }
    }

