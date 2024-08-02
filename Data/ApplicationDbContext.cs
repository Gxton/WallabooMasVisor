using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using System.Linq.Expressions;
using System.Reflection;
using Wallabo.Entities;
using Wallaboo.Entities;
using Wallaboo.Interfaces;
using Wallaboo.Services;


namespace Wallaboo.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        private string tenantId;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IServiceTenant serviceTenant)
            : base(options)
        {
            tenantId = serviceTenant.Obtenertenant();
        }
        //Gestion Tenant

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Cliente>(entityTypeBuilder =>
            //{
            //    entityTypeBuilder.ToTable("Usuarios");
            //});
            //modelBuilder.Entity<Pais>().HasData(new Pais { id = 1, NombrePais = "Argentina" });
            //modelBuilder.Entity<Provincia>().HasData(new Provincia() { PaisId=1, id=1, NombreProvincia="Buenos Aires"});
            //modelBuilder.Entity<Ciudad>().HasData(new Ciudad() {ProvinciaId=1, Id=1, NombreCiudad="Mar del Plata" });

            //Aplica las configuraciones del proyecto (en este caso carpeta Configuraciones (Interfaces))
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entidad in modelBuilder.Model.GetEntityTypes())
            {
                var tipo = entidad.ClrType;
                if (typeof(IEntidadTenant).IsAssignableFrom(tipo))
                {
                    var metodo = typeof(ApplicationDbContext).GetMethod(nameof(ArmarFiltroGlobalTenant),
                        BindingFlags.NonPublic | BindingFlags.Static)?.MakeGenericMethod(tipo);

                    var filtro = metodo?.Invoke(null, new object[] { this })!;
                    entidad.SetQueryFilter((LambdaExpression)filtro);
                    entidad.AddIndex(entidad.FindProperty(nameof(IEntidadTenant.TenantId))!);
                }
                else if (tipo.DebeSaltarValidacionTenant())
                {
                    continue;
                }
                else
                {
                    throw new Exception($"la entidad {entidad} no ha sido marcada com tenant o comun");
                }
            }
        }
        public static LambdaExpression ArmarFiltroGlobalTenant<TEntidad>(ApplicationDbContext context)
                where TEntidad : class, IEntidadTenant
        {
            Expression<Func<TEntidad, bool>> filtro = x => x.TenantId == context.tenantId;
            return filtro;
        }
        public DbSet<Anuncio> Anuncios => Set<Anuncio>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Pais> Paises => Set<Pais>();
        public DbSet<Provincia> Provincias => Set<Provincia>();
        public DbSet<Ciudad> Ciudades => Set<Ciudad>();
        //public DbSet<IdentityUser> Usuarios => Set<IdentityUser>();
    }
}
